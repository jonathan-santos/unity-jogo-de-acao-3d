using UnityEngine;
using UnityEngine.AI;

public class Dano : MonoBehaviour
{
    public float dano = 1f;
    public float afastar = 0f;
    public string tagDeObjetosColidiveis = "";
    public bool autoDestruirEmColisao = false;

    Vida vidaDeObjetoColidido;
    Rigidbody rbDeObjetoColidido;
    NavMeshAgent navMeshDeObjetoColidido;

    void OnTriggerEnter(Collider colisao)
    {
        AplicarDano(colisao.gameObject);
    }

    void AplicarDano(GameObject objetoColidido)
    {
        if (objetoColidido.tag == tagDeObjetosColidiveis)
        {
            vidaDeObjetoColidido = objetoColidido.GetComponent<Vida>();
            rbDeObjetoColidido = objetoColidido.GetComponent<Rigidbody>();
            navMeshDeObjetoColidido = objetoColidido.GetComponent<NavMeshAgent>();

            if (vidaDeObjetoColidido != null)
                vidaDeObjetoColidido.MudarVida(-dano);

            if (navMeshDeObjetoColidido != null && navMeshDeObjetoColidido.enabled)
            {
                navMeshDeObjetoColidido.enabled = false;
                rbDeObjetoColidido.isKinematic = false;
                Invoke("ReativarNavMeshDeObjetoColidido", 0.25f);
            }

            if (rbDeObjetoColidido != null)
            {
                var direcaoParaAfastar = objetoColidido.transform.position - transform.position;
                rbDeObjetoColidido.AddForce(direcaoParaAfastar * afastar, ForceMode.Impulse);
            }
        }

        if (autoDestruirEmColisao)
            Destroy(gameObject);
    }

    void ReativarNavMeshDeObjetoColidido()
    {
        if(navMeshDeObjetoColidido != null)
        {
            navMeshDeObjetoColidido.enabled = true;
            rbDeObjetoColidido.isKinematic = true;
        }
    }

    void OnDestroy()
    {
        if (navMeshDeObjetoColidido != null && !navMeshDeObjetoColidido.enabled)
            ReativarNavMeshDeObjetoColidido();
    }
}