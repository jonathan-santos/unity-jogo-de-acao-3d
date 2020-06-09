using UnityEngine;
using UnityEngine.UI;

public class Vida : MonoBehaviour
{
    public float vidaMaxima = 3;
    public float vida;
    public bool destruirComZeroVida = true;
    public Slider barraDeVida;

    void Start()
    {
        vida = vidaMaxima;

        if (barraDeVida != null)
        {
            barraDeVida.maxValue = vidaMaxima;
            barraDeVida.value = vida;
        }
    }

    public void MudarVida(float valor)
    {
        vida += valor;

        if (barraDeVida != null)
            barraDeVida.value = vida;

        if (vida < 1 && destruirComZeroVida)
            Destroy(this.gameObject);
    }
}