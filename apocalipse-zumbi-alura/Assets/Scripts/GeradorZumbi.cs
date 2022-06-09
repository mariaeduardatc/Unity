using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorZumbi : MonoBehaviour
{
    public GameObject Zumbi;
    private float contadorTempo = 0; //relógio para utilizarmos para a geração de zumbis por segundo
    public float TempoGerarZumbi = 1; //tempo denominado para a geração de zumbis. utilizado no if.
    public LayerMask LayerZumbi;
    private float distanciaDeGeracao = 3;  
    private float distanciaDoJogadorParaGeracao = 20;
    private GameObject jogador;
    private int quantidadeMaximaDeZumbisVivos = 2;
    private int quantidadeDeZumbisVivos;
    private float tempoProximoAumentoDeDificuldade = 25;
    private float contadorDeAumentarDificuldade;

    void Start(){
        
        jogador = GameObject.FindWithTag(Tags.Jogador);
        contadorDeAumentarDificuldade = tempoProximoAumentoDeDificuldade;
        
        for(int i = 0; i < quantidadeMaximaDeZumbisVivos; i++){
            StartCoroutine(GerarNovoZumbi());
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool possoGerarZumbisPelaDistancia = Vector3.Distance(transform.position, jogador.transform.position) > distanciaDoJogadorParaGeracao;

        if (possoGerarZumbisPelaDistancia == true && 
            quantidadeDeZumbisVivos < quantidadeMaximaDeZumbisVivos){ //limitação da criação de zumbis pela distância a personagem E quantidade de zumbis gerados por gerador
            
            contadorTempo += Time.deltaTime; //contadorTempo = contadorTempo + Time.deltaTime; Time.deltaTime é a variável que o Unity nos fornece
            // para fazer as coisas ficarem em segundos

            if(contadorTempo >= TempoGerarZumbi){ //cria uma condição para a criação de zumbis
                
                StartCoroutine(GerarNovoZumbi());
                contadorTempo = 0; //zera o contadorTempo 
            }

        }

        
        if(Time.timeSinceLevelLoad > contadorDeAumentarDificuldade){
            
            contadorDeAumentarDificuldade = Time.timeSinceLevelLoad + tempoProximoAumentoDeDificuldade; // nova condição para aumentar o nível do jogo
            quantidadeMaximaDeZumbisVivos++;
        }  

    }
    
    void OnDrawGizmos(){
        
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, distanciaDeGeracao);
    }

    IEnumerator GerarNovoZumbi(){//esse tipo de método é chamado de Coroutine
        
        Vector3 posicaoDeCriacao = AleatorizarPosicao();

        Collider[] colisoes = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi); //Collider[] acumula vários vetores
        //Physics.Overlap testa, nesse caso em uma raio = 1 e na layer zumbi, se há algum objeto físico, com rigidbody etc, naquele espaço 

        while (colisoes.Length > 0){
            
            posicaoDeCriacao = AleatorizarPosicao();

            colisoes = Physics.OverlapSphere (posicaoDeCriacao, 1, LayerZumbi);

            yield return null; //tipo de retorno do IEnumerator. nesse caso, o null faz com que a Unity não trave em um frame tentando achar uma nova
            //posição, ela assim, posibilita que prossigamos com os frames e testemos a condição do while nesse novo frame.
        }

        ControlaZumbi zumbi = Instantiate(Zumbi, posicaoDeCriacao, transform.rotation).GetComponent<ControlaZumbi>(); //gerador de zuumbis ligado ao ponto escolhido pelo empty boject da unity
        zumbi.meuGerador = this; //this faz ref. a esse script o GeradorZumbi
        quantidadeDeZumbisVivos++;
    }
    

    Vector3 AleatorizarPosicao(){ //vecto3 usado por causa do return

        Vector3 posicao = Random.insideUnitSphere * distanciaDeGeracao; //aleatoriza uma posição dentro de uma esfera de raio 1
        
        posicao += transform.position;

        posicao.y = 0; //cancela o movimento no eixo y

        return posicao; 
    }

    public void DiminuirQuantidadeDeZumbisVivos(){

        quantidadeDeZumbisVivos--;
    }
}
