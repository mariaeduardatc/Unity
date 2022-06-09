using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaZumbi : MonoBehaviour, IMatavel
{
    public GameObject Jogador; //pela Unity vamos colocar o Persongem dentro da variavel jogador
    //public float Velocidade = 5; // determina a velocidade do Zumbi
    private MovimentoPersonagem movimentoInimigo;
    private AnimacaoPersonagem animacaoInimigo;
    private Status statusInimigo;
    public AudioClip SomDaMorte;
    private Vector3 direcao;
    private Vector3 posicaoAleatoria;
    private float contadorVagar;
    private float tempoEntrePosicoesAleatorias = 4;
    private float distanciaMinimimaEntreAmbos = 0.05f;
    public GameObject PrefabKitMedico;
    private float porcentagemGerarKitMedico = 0.3f;
    private ControlaInterface scriptControlaInterface;
    [HideInInspector]
    public GeradorZumbi meuGerador;
    public GameObject ParticulaSangueZumbi;
   
    // Start is called before the first frame update
    void Start()// chamdao logo no ínicio, uma vez, cada zumbi novo irá ter isso como primeira informação
    {
        Jogador = GameObject.FindWithTag(Tags.Jogador);
        
        movimentoInimigo = GetComponent<MovimentoPersonagem>();
        animacaoInimigo = GetComponent<AnimacaoPersonagem>();
        AleatorizarZumbi();
        statusInimigo = GetComponent<Status>();
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface; 
    }


    void FixedUpdate(){
        
        float distancia = Vector3.Distance(transform.position, Jogador.transform.position);
        //Vecto3.Distance calcula a dist. entre duas coisas. Nesse caso, é a distância entre o zumbi e a heroina. Esse cálculo é utilizado para fazer o zumbi parar
        movimentoInimigo.Rotacionar(direcao);
        animacaoInimigo.Movimentar(direcao.magnitude); //transforma o vector3 em um valor só (seu tamanho) -> float
        

        if (distancia > 15){
            Vagar();
        }
        else if (distancia > 2.5){
        direcao = Jogador.transform.position - transform.position; // subtraindo a posição do jogador da posição da heroina
        movimentoInimigo.Movimentar(direcao.normalized, statusInimigo.Velocidade);
        animacaoInimigo.Atacar(false);
        } else {
            //zumbi ataca
            direcao = Jogador.transform.position - transform.position;
            animacaoInimigo.Atacar(true); 
        }

    }

    void Vagar(){

        contadorVagar -= Time.deltaTime; //calculador para a geração de posições aleatórias do zumbi
        if(contadorVagar <= 0){
            posicaoAleatoria = AleatorizarPosicao();
            contadorVagar += tempoEntrePosicoesAleatorias + Random.Range(-1f, 1f);
        }
        
        bool ficouPertoSuficiente = Vector3.Distance (posicaoAleatoria, transform.position) <= distanciaMinimimaEntreAmbos;
        if (ficouPertoSuficiente == false){ //(posicaoAleatoria - transform.position != 0) não pode serusado como parâmetro, tendo em vista que ambas as variáveis são Vector3 e 0 é um int
            //movimentação do zumbi
            direcao = posicaoAleatoria - transform.position; //o zumbi pega a posição aleatoria e diminui a sua própria
            movimentoInimigo.Movimentar(direcao, statusInimigo.Velocidade); //comando que indica o movimento do zumbi
        }
        
    }

    Vector3 AleatorizarPosicao(){ //vecto3 usado por causa do return
        Vector3 posicao = Random.insideUnitSphere * 10; //aleatoriza uma posição dentro de uma esfera de raio 1
        posicao += transform.position;
        posicao.y = transform.position.y; //cancela o movimento no eixo y

        return posicao; 
    }
    
    void AleatorizarZumbi ()
    {
        int geraTipoZumbi = Random.Range(1, transform.childCount);
        transform.GetChild(geraTipoZumbi).gameObject.SetActive(true);
    }

    void GolpeFatal(){//void criado para o ataque do zumbi
    
    //Time.timeScale = 0; //faz com ue quando o GolpeFatal() rodar, o tempo do jogo pare, ou seja, na prática quando o zumbi fizer o
    //movimento de ataque definido no evento na animação, o jogo para/acaba.
    //Jogador.GetComponent<Movimentacao>().TextoGameOver.SetActive(true); //jogador.GetComponet está entrando na minha personagem na unity
    // e acessando especificamente o script "movimentação" e o gameobject publico denominado TextoGameOver. Jogador é o nome da variavel 
    // pública que eu criei e na unity coloquei "personagens". SetActive serve p defirnirmos se
    //ativamos ou não algo dentro da unity. No caso, meu texto está desligado e deve ser ativado (true) apensa quando a timescale for 0.
    //Jogador.GetComponent<Movimentacao>().Vivo = false; //informação tirada do script ddo jogador: define que após o golpe do zumbi, o jogador
    //não está vivo.
    int dano = Random.Range(20, 30);
    Jogador.GetComponent<Movimentacao>().TomarDano(dano); //crio nesse script o parâmetro para ser levado em conta no script da
    // jogadora para a perda de vida

    //o GolpeFatal está ligado a um evento de animação! ou seja, na animação de ataque, trazida para o código no FixedUpdate, existe um evente criado chamado
    //GolpeFatal. Isso faz com que, quando o objeto do jogo a qual essa animação está ligada fizer o movimento "x", o método GolpeFatal será chamado. Nesse caso,
    //Quando o zumbi estiver na metade +- do movimento de ataque da animação 'ataque', o método GolpeFatal é chamado para então o zumbi conseguir infligir um dano no Jogador.
    }

    public void TomarDano(int dano){

        statusInimigo.Vida -= dano;
        if(statusInimigo.Vida  <= 0){
            Morrer();
        }
    }

    public void ParticulaSangue(Vector3 posicao, Quaternion rotacao){
        Instantiate(ParticulaSangueZumbi, posicao, rotacao);
    }

    public void Morrer(){
        
        Destroy(gameObject, 2);//se a bala bater em um objeto com tag de Inimigo, ela irá destruí-lo única e exclusivamente
                //isso evita que a bala destrua tudo ao seu redor. O objetoDeColisao é extraído do Collider e .gameobject faz com que tragamos p 
                //o código sua 'natureza' dentro da unity. Faça referência p a bala destruir >aquele< objeto dentro da unity.

        animacaoInimigo.Morrer();
        movimentoInimigo.Morrer();
        this.enabled = false; //desabilita o script
        
        ControlaAudio.instancia.PlayOneShot(SomDaMorte);
        VerificarGeracaoKitMedico(porcentagemGerarKitMedico);
        scriptControlaInterface.AtualizarQuantidadeZumbisMortos();
        meuGerador.DiminuirQuantidadeDeZumbisVivos();
    
    }

    void VerificarGeracaoKitMedico (float porcentagemGeracao){

        if(Random.value <= porcentagemGeracao){ //Random.value gera um valor aleatório entre 0 e 1
            Instantiate(PrefabKitMedico, transform.position, Quaternion.identity);
       }
    }
}
