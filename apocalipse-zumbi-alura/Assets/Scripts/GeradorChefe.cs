using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorChefe : MonoBehaviour
{
    private float tempoParaProximaGeracao = 0;
    private float tempoEntreGeracoes = 05;
    public GameObject ChefePrefab;
    private ControlaInterface scriptControlaInterface;
    public Transform[] PosicoesPossiveisDeGeracao;
    private Transform jogador;

    private void Start(){

        tempoParaProximaGeracao = tempoEntreGeracoes;
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
        jogador = GameObject.FindWithTag(Tags.Jogador).transform;
    }

    private void Update() {

        if(Time.timeSinceLevelLoad > tempoParaProximaGeracao){

            Vector3 posicaoDeCriacao = CalucularPosicaoMaisDistanteDoJogador();

            Instantiate(ChefePrefab, posicaoDeCriacao, Quaternion.identity);
            scriptControlaInterface.AparecerTextoChefeCriado();
            tempoParaProximaGeracao = Time.timeSinceLevelLoad + tempoEntreGeracoes; // note que tempoEntreGeracoes é 60, devido ao start!
        }
    }

    void OnDrawGizmos(){
        
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, 2);
    }

    Vector3 CalucularPosicaoMaisDistanteDoJogador(){
        Vector3 posicaoDeMaiorDistancia = Vector3.zero;
        float maiorDistancia = 0;

        foreach (Transform posicao in PosicoesPossiveisDeGeracao){ //tradução: para cada elemento dotipo transform dentro da coleção POsicoesPOssiveisDeGeracao
            //posicao vai mudando de valor de elemento, ela vai assumindo qual elemento está na vez de acordo com a ordem definida na unity na coleção PosicoesPossiveisDeGeracao
            float distanciaEntreOJogador = Vector3.Distance(posicao.position, jogador.position);
            if(distanciaEntreOJogador> maiorDistancia){
                maiorDistancia = distanciaEntreOJogador;
                posicaoDeMaiorDistancia = posicao.position;
            }
        }



        return posicaoDeMaiorDistancia;
    }

}
