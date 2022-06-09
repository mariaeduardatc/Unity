using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacaoPersonagem : MonoBehaviour
{
private Animator meuAnimator;

void Awake(){
    
    meuAnimator = GetComponent<Animator>();
}

public void Atacar (bool estado){

    meuAnimator.SetBool("Atacando", estado);// GetComponent serve para pegarmos algo do enviroment da unity 
            //e jogar para o algoritmo. No SetBool colocamos o nome dado a condição criada na Unity (nesse caso "Atacando") e 
            //qual valor ela vai assumir.
}

public void Movimentar (float valorMovimento){

    meuAnimator.SetFloat("Movendo", valorMovimento);//movendo é um parametro do animator dentro da unity
}

public void Morrer(){
    meuAnimator.SetTrigger("Morrer");
}
}
