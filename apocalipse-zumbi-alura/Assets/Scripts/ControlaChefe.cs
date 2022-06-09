using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControlaChefe : MonoBehaviour, IMatavel
{
    private Transform jogador; //não é um gameobject pq estamos pulando um passo, que será retomado no void start
    private NavMeshAgent agente;
    private Status statusChefe;
    private AnimacaoPersonagem animacaoChefe;
    private MovimentoPersonagem movimentoChefe;
    public GameObject KitMedico;
    public Slider sliderVidaChefe;
    public Color CorDaVidaMaxima, CorDaVidaMinima;
    public Image SliderImagem;
    public GameObject ParticulaSangueZumbi;
   

    void Start(){

        statusChefe = GetComponent<Status>();
        animacaoChefe = GetComponent<AnimacaoPersonagem>();
        movimentoChefe = GetComponent<MovimentoPersonagem>();
        jogador = GameObject.FindWithTag(Tags.Jogador).transform;
        agente = GetComponent<NavMeshAgent>();
        agente.speed = statusChefe.Velocidade;
        sliderVidaChefe.maxValue = statusChefe.VidaInicial;
        AtualizarInterface();
    }

    void Update(){

        agente.SetDestination(jogador.position);// destino de persegução do chefe
        animacaoChefe.Movimentar(agente.velocity.magnitude);

        if(agente.hasPath == true){

            bool estouPertoDoJogador = agente.remainingDistance <= agente.stoppingDistance; //estratégias de comaparação de sitâncias que existem já na AI

            if(estouPertoDoJogador){

                animacaoChefe.Atacar(true);
                Vector3 direcao = jogador.position - transform.position;
                movimentoChefe.Rotacionar(direcao);
            }else{

                animacaoChefe.Atacar(false);
            }
        }
    }

    void GolpeFatal(){

        int dano = Random.Range(30, 40);
        jogador.GetComponent<Movimentacao>().TomarDano(dano);
    }

    public void TomarDano(int dano){

        statusChefe.Vida -= dano;
        AtualizarInterface();

        if(statusChefe.Vida <= 0){

            Morrer();
        }
    }

    public void ParticulaSangue(Vector3 posicao, Quaternion rotacao){
        Instantiate(ParticulaSangueZumbi, posicao, rotacao);
    }

    public void Morrer(){

        animacaoChefe.Morrer();
        movimentoChefe.Morrer();
        this.enabled = false;
        agente.enabled = false;
        Instantiate (KitMedico, transform.position, Quaternion.identity);
        Destroy(gameObject, 2);
    }

    void AtualizarInterface(){

        sliderVidaChefe.value = statusChefe.Vida;
        float porcentagemDaVida = (float)statusChefe.Vida / statusChefe.VidaInicial; 
        Color corDaVida = Color.Lerp(CorDaVidaMinima, CorDaVidaMaxima, porcentagemDaVida); // Lerp é uma interpolação linear
        SliderImagem.color = corDaVida;
    }
}
