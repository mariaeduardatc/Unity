using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{

public float Velocidade = 20;
private Rigidbody rigidbodyBala;
public AudioClip SomDaMorte;
private int DanoDoTiro = 1;
    
    void Start(){

        rigidbodyBala = GetComponent<Rigidbody>();
        Velocidade = - Velocidade;   
     
    }
     
     // Update is called once per frame
    void FixedUpdate()
    {
        rigidbodyBala.MovePosition
        (rigidbodyBala.position + transform.forward * Velocidade * Time.deltaTime);
    }

        void OnTriggerEnter(Collider objetoDeColisao){//pega a informação de que objeto a bala como trigger colide
            
            Quaternion rotacaoOpostaABala = Quaternion.LookRotation(-transform.forward);
            switch (objetoDeColisao.tag){
                //Destroy(objetoDeColisao.gameObject);//se a bala bater em um objeto com tag de Inimigo, ela irá destruí-lo única e exclusivamente
                //isso evita que a bala destrua tudo ao seu redor. O objetoDeColisao é extraído do Collider e .gameobject faz com que tragamos p 
                //o código sua 'natureza' dentro da unity. Faça referência p a bala destruir >aquele< objeto dentro da unity.
                //ControlaAudio.instancia.PlayOneShot(SomDaMorte);
                
                case "Inimigo":
                    ControlaZumbi inimigo = objetoDeColisao.GetComponent<ControlaZumbi>();
                    inimigo.TomarDano(DanoDoTiro);
                    inimigo.ParticulaSangue(transform.position, rotacaoOpostaABala);
                break;

                case "Chefe":
                    ControlaChefe chefe = objetoDeColisao.GetComponent<ControlaChefe>();
                    chefe.TomarDano(DanoDoTiro);
                    chefe.ParticulaSangue(transform.position, rotacaoOpostaABala);
                break;
            }
            Destroy(gameObject);//destrói a bala (gameobject sozinho dentro de um código é o obejto ao qual o código é ligado dentro da unity.)
        }
    
}
