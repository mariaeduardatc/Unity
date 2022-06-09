using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    private Rigidbody meuRigidboby;

    void Awake(){
        meuRigidboby = GetComponent<Rigidbody>();
    }

    public void Movimentar (Vector3 direcao, float velocidade)
    {
        meuRigidboby.MovePosition(
            meuRigidboby.position + 
            (direcao * velocidade * Time.deltaTime)); // movimentação do zumbi
        //direcao.normalized serve para que o  zumbi ande de 1 em 1 quadrado e não a dist. total entre ele e a heroina
        
    }

    public void Rotacionar (Vector3 direcao){

        Quaternion novaRotacao = Quaternion.LookRotation(direcao);
        meuRigidboby.MoveRotation(novaRotacao); 
        //Quaternion é uma estrutura de variável utilizada para rotação, usa os eixos x/y/z
        //A rotação definida por Quaternion.LookRotation() é atribuida as posições de direcao
        //MoveRotationLookRotation que calcula a rotação
    }

    public void Morrer(){

        meuRigidboby.constraints = RigidbodyConstraints.None; //tirei todas as travas do rigidbody do zumbi
        meuRigidboby.velocity = Vector3.zero; // tirou a velocidade do zumbi e a gravidade vai atuar sobre ele
        meuRigidboby.isKinematic = false;
        GetComponent<Collider>().enabled = false;  //vai pegar qualquer collider, capsule etc
    }
}
