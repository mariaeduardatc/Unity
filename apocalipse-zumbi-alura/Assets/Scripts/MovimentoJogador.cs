using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoJogador : MovimentoPersonagem //herança
{
   public void RotacaoJogador(LayerMask MascaraChao){

       Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition); //referência de rotação da heroína. Ray sai da camera principal, 
        //pega um ponto da tela e o transforma em um raio(ScreenPointToRay), entre parentesis colocamos qual ponto da tela queremos apontar 
        //o raio, q será direcionado pelo mouse (Input.mousePosition)
        Debug.DrawRay(raio.origin, raio.direction * 100, Color.red); //Debug procura erros na unity e o DrawRay desenha o raio fazendo com que consigamos 
        //enxergá-lo na unity. Entre () precismaos dizer onde ele {o raio} começa e a direção que ele está. Tais dados estão armazenados na var raio,
        //por isso colocamos raio.origin p onde começa e raio.direction p a direção que o raio aponta. raio.direction é multiplicado por 100 
        //p crescer 

        RaycastHit impacto;
        if(Physics.Raycast(raio, out impacto, 100, MascaraChao)){//a var raio não pode ser usada em if, por isso criamos o impacto. RaycastHit indica onde o raio "bate"
        //em algo dentro da unity. Out é usado pq a var impacto não tem valor ainda, será adquirido um valor p ela dentro do if.

        Vector3 posicaoMiraJogador = impacto.point - transform.position; // Vector3 guarda a posição de impacto. impacto.point pega
        //ponto da var impacto, já que o raio já está tocando um ponto no chão. transform.position subtrai a posição do jogador, obtendo
        //o ponto de impacto com base na posição dele, como fizemos com os zumbis.
        posicaoMiraJogador.y = transform.position.y; // cancela o eixo y do raio, considerando q ele toca o chão e o jogador tá em cima
        //dele. assim igualamos a altura de ponto de impacto e jogador, fazendo com q ele n olhe p baixo ou p cima
        
        Rotacionar(posicaoMiraJogador); //vem do script MovimentoPersonagem
        }
   }
}
