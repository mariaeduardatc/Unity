using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitMedico : MonoBehaviour
{
    private int quantidadeCura = 15;
    private float tempoDestruir = 5;


    void Start() {
        Destroy(gameObject, tempoDestruir); //modo do Destroy que leva em consideração um tempo para a destruição do objeto desde sua criação
    }

    void OnTriggerEnter (Collider objetoDeColisao){

        if(objetoDeColisao.tag == "Jogador"){
            objetoDeColisao.GetComponent<Movimentacao>().CurarVida(quantidadeCura);
            Destroy(gameObject);
        }
    }
}
