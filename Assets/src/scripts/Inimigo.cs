using UnityEngine;
using UnityEngine.AI;

public class Inimigo : MonoBehaviour
{
    public float distanciaDeAgressividade = 5f;
    NavMeshAgent agente;
    GameObject jogador;

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("jogador");
        agente = GetComponent<NavMeshAgent>();

        InvokeRepeating("IrParaJogador", 0, 0.5f);
    }

    void IrParaJogador()
    {
        var distanciaParaJogador = Vector3.Distance(jogador.transform.position, transform.position);
        if (agente.enabled && distanciaParaJogador <= distanciaDeAgressividade)
            agente.destination = jogador.transform.position;
    }

    void LateUpdate()
    {
        var direcaoDoJogador = transform.TransformDirection(jogador.transform.position);
        transform.eulerAngles = new Vector3(0, direcaoDoJogador.y, 0);
    }
}