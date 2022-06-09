using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //responsável pelas cenas do jogo. è utilizado para reiniciarmos o jogo.
//Assim, estabelecemos que usaremos (using) a parte da Unity (UnityEngine) que lida com as cenas, (SceneManagement).

public class Movimentacao : MonoBehaviour, IMatavel, ICuravel
{

    //public float velocidade = 10;

    Vector3 direcao;
    public LayerMask MascaraChao; //criação de camada exclusiva para o chao do jogo, para que nosso raycast bata apenas nele e evite outros 
    //objetos da unity.
    public GameObject TextoGameOver; //criado para adicionarmos o texto 'game over' na unity e usarmos no script do zumbi. public creations 
    // podem acessadas em outros scripts do jogo.
    //não é mais necessária a partir da criação da var Vida: public bool Vivo = true; //a variável define que o jogador está inicialmente vivo. será utilizada no script do zumbi.
    //private Rigidbody rigidbodyJogador;
    //private Animator animatorJogador;
    //public int Vida = 100; //vida do jogador (contador)
    public ControlaInterface scriptControlaInterface;
    public AudioClip SomDeDano;
    private MovimentoJogador meuMovimentoJogador;
    private AnimacaoPersonagem animacaoJogador;
    public Status statusJogador; //public para ser usada no ControlaInterface

    private void Start()// método criado para quando reiniciarmos o jogo, voltarmos para a tela inicial e não para uma timescale = 0.
    {
        //rigidbodyJogador = GetComponent<Rigidbody>();
        //animatorJogador = GetComponent<Animator>();
        meuMovimentoJogador = GetComponent<MovimentoJogador>();
        animacaoJogador = GetComponent<AnimacaoPersonagem>();
        statusJogador = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {   
        //Inputs do Jogador - Guardando teclas apertadas
        float eixoX = Input.GetAxis("Horizontal");
        float eixoZ = Input.GetAxis("Vertical");

        direcao = new Vector3(eixoX, 0, eixoZ);
        
        
        // parte excluida: transform.Translate(direcao * velocidade * Time.deltaTime);
        
        //Animações do Jogador
        // LINHAS APAGADAS:
        //if(direcao != Vector3.zero){ //Vector3.zero é como a Unity define todos os 3 valores do Vector como 0, ou seja, 
        //o personagem está parado
            //animatorJogador.SetBool("Movendo", true);//Animator não é um componente obrigatório de todos os objetos, 
            //por isso precisamos utilizar o GetComponent<> 
                                        // .SetBool() irá dar um valor boolean para algo
        //} else {
          //animatorJogador.SetBool("Movendo", false);  
        //}

        animacaoJogador.Movimentar(direcao.magnitude);//retorna o tamnho do vector3
        
        //if(statusJogador.Vida <= 0){// quando a personagem estiver morta o código a seguir deve ser rodado
           // if(Input.GetButtonDown("Fire1")){// nessas condições quando o usuário clicar no mouse com o botão esquerdo, o jogo irá reiniciar
                
                //SceneManager.LoadScene("Game");//scenemanager é definido lá em cima em using. LoadScene é para carregar a cena inicial do jogo
                //"Game" é o nome do jogo que fica no topo da hierarchy da unity.
            //}
        //}

        
    }
    void FixedUpdate(){ //roda num tempo fixo 0,02seg.

        meuMovimentoJogador.Movimentar(direcao, statusJogador.Velocidade); //apesar desse método ser do "MovimentoPersonagem" 
        //podemos usar com o MovimentoJogador, pq esse tem o primeiro como herança
        
        meuMovimentoJogador.RotacaoJogador(MascaraChao);

    }

    public void TomarDano(int dano){ //isso se chama método! podemos criar quantos quisermos

        statusJogador.Vida -= dano; //essa informação vem do int dano, ligado ao script ControlaZumbi
        scriptControlaInterface.AtualizarSliderVidaJogador(); //ligado ao script ControlaInterface
        ControlaAudio.instancia.PlayOneShot(SomDeDano);

        if(statusJogador.Vida <= 0){ // esse código estava originalmente no ControlaZumbi na versão 1 do jogo
            
            Morrer();
        }
        
    }

    public void Morrer(){
        scriptControlaInterface.GameOver();
    }

    public void CurarVida (int quantidadeDeCura){
        
        statusJogador.Vida += quantidadeDeCura;

        if(statusJogador.Vida > statusJogador.VidaInicial){
            statusJogador.Vida = statusJogador.VidaInicial;
        }
        scriptControlaInterface.AtualizarSliderVidaJogador();
    }
}
