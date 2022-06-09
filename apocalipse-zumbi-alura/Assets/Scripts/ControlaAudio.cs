using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaAudio : MonoBehaviour
{

    private AudioSource meuAudioSource; //guarda o AudioSource do nosso objeto vazio
    public static AudioSource instancia; //essa variavel apesar de poder ser utilizada em outros scripts, manterá seu valor estático


    // Start is called before the first frame update
    void Awake() // roda antes do start e certifica que a musica toque antes do inicio do jogo 
    {
        //SINGLETON BÁSICO - (pattern)
        meuAudioSource = GetComponent<AudioSource>();
        instancia = meuAudioSource;

    }
}
