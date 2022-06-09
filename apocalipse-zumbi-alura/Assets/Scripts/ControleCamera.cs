using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCamera : MonoBehaviour
{
    public GameObject Jogador;
    Vector3 distDiferenca;

    // Start is called before the first frame update
    void Start()
    {
        distDiferenca = transform.position - Jogador.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Jogador.transform.position + distDiferenca;
        
    }
}
