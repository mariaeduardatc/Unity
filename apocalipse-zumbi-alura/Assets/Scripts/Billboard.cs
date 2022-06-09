using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update(){

       transform.LookAt(transform.position + Camera.main.transform.forward);  //Camera.main acessa a camera principal do jogo, .transform suas infos de posição etc, e o .forward o eixo z, que é o olho propriamente dito da camera quando observamos pela unity 
    }
}
