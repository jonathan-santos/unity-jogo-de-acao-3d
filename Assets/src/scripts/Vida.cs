using UnityEngine;

public class Vida : MonoBehaviour
{
    public float vidaMaxima = 3;
    public float vida;
    public bool destruirComZeroVida = true;

    void Start()
    {
        vida = vidaMaxima;
    }

    public void MudarVida(float valor)
    {
        vida += valor;

        if (vida < 1 && destruirComZeroVida)
            Destroy(this.gameObject);
    }
}