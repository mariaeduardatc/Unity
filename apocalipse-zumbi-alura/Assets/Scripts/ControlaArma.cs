using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaArma : MonoBehaviour
{
    public GameObject Bala; // coloca o Prefab da bala nesse espaço.
    public GameObject CanoDaArma; //Feita para pegarmos as infos do cano
    public AudioClip SomDeTiro;
    


    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){
         Instantiate(Bala, CanoDaArma.transform.position, CanoDaArma.transform.rotation);   
         ControlaAudio.instancia.PlayOneShot(SomDeTiro);
        }
    }
}
