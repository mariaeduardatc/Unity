using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //deixa os componentes da UI disponíveis para o código
using UnityEngine.SceneManagement;

public class ControlaInterface : MonoBehaviour
{

    private Movimentacao scriptMovimentacao;
    public Slider SliderVidaJogador; //para colocar o slider no inspector
    public GameObject PainelDeGameOver;
    public Text TextoTempoDeSobrevivencia;
    public Text TextoPontuacaoMaxima;
    private float tempoPontuacaoMaximaSalva;
    private int quantidadeDeZumbisMortos;
    public Text TextoQuantidadeDeZumbisMortos;
    public Text TextoChefeAparece;

    // Start is called before the first frame update
    void Start()
    {
        scriptMovimentacao = GameObject.FindWithTag(Tags.Jogador).GetComponent<Movimentacao>(); //pegar o script de movimentacao
        SliderVidaJogador.maxValue = scriptMovimentacao.statusJogador.Vida; //assimila o valor da vida do script do jogador com o valor max que 
        //a Slider pode assumir
        
        AtualizarSliderVidaJogador(); //chama o void abaixo
        Time.timeScale = 1;
        tempoPontuacaoMaximaSalva = PlayerPrefs.GetFloat("PontuacaoMaxima"); //"Get" pq está pegando a info do PontuacaoMaxima() que foi gerada em um "Set"
    }

    public void AtualizarSliderVidaJogador ()
    {
        SliderVidaJogador.value = scriptMovimentacao.statusJogador.Vida; //faz com que o valor do slider seja igual ao da vida
    }

    public void GameOver(){
        PainelDeGameOver.SetActive(true);
        Time.timeScale = 0;

        int minutosJogados = (int)(Time.timeSinceLevelLoad / 60);
        int segundosJogados = (int)(Time.timeSinceLevelLoad % 60); //calcula o que sobrou da divisão % = módulo
        TextoTempoDeSobrevivencia.text = "Você sobreviveu por " + minutosJogados + "min e " + segundosJogados + "s";
        PontuacaoMaxima(minutosJogados, segundosJogados);
    }

    void PontuacaoMaxima(int min, int seg){

        if(Time.timeSinceLevelLoad > tempoPontuacaoMaximaSalva){
            tempoPontuacaoMaximaSalva = Time.timeSinceLevelLoad;
            TextoPontuacaoMaxima.text = string.Format("Seu melhor tempo é {0}min e {1}s", min, seg);
            PlayerPrefs.SetFloat("PontuacaoMaxima", tempoPontuacaoMaximaSalva); //PlayerPrefs salvam infos do jogo na plataforma em que o usuário está jogando
            //essaas informações ficam salvas mesmo depois da saída dele da página de jogo.
        }

        if(TextoPontuacaoMaxima.text == "")
    {
        min= (int)(tempoPontuacaoMaximaSalva / 60);
        seg = (int)(tempoPontuacaoMaximaSalva % 60);
        TextoPontuacaoMaxima.text = string.Format("Seu melhor tempo é {0}min e {1}s", min, seg);
    }
    }

    public void Reiniciar(){
        SceneManager.LoadScene("Game");
    }

    public void AtualizarQuantidadeZumbisMortos() {

        quantidadeDeZumbisMortos++;
        TextoQuantidadeDeZumbisMortos.text = string.Format("x {0}", quantidadeDeZumbisMortos);
    }

    public void AparecerTextoChefeCriado() {
        StartCoroutine(DesaparecerTexto(2, TextoChefeAparece));

    }

    IEnumerator DesaparecerTexto (float tempoDeSumico, Text textoParaSumir) { //coroutine vem de co-routine. é uma função que será atualizada a cada frame. 
    //poderia ser uma função/método colocado no FixedUpdate, mas é interessante criar coroutines para organização etc

        textoParaSumir.gameObject.SetActive(true);
        Color corTexto = textoParaSumir.color;
        corTexto.a = 1;
        textoParaSumir.color = corTexto;
        yield return new WaitForSeconds(1); 
        float contador = 0;

        while(textoParaSumir.color.a > 0){
            contador += (Time.deltaTime / tempoDeSumico);
            corTexto.a = Mathf.Lerp(1, 0, contador);
            textoParaSumir.color = corTexto;

            if(textoParaSumir.color.a <= 0){
                textoParaSumir.gameObject.SetActive(false);
            }
        }
        yield return null; //para a leitura do método
    }
}
