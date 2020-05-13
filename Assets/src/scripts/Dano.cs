using UnityEngine;

public class Dano : MonoBehaviour
{
    public float dano = 1f;
    public float afastar = 0f;
    public string tagDeObjetosColidiveis = "";
    public bool autoDestruirEmColisao = false;

    Vida vidaDeObjetoColidido;
    Rigidbody rbDeObjetoColidido;

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

            if(vidaDeObjetoColidido != null)
                vidaDeObjetoColidido.MudarVida(-dano);

            if(rbDeObjetoColidido != null)
                rbDeObjetoColidido.AddForce(transform.forward * afastar, ForceMode.Impulse);
        }

        if (autoDestruirEmColisao)
            Destroy(gameObject);

        
    }
}
