using UnityEngine;

public class VerDeFrenteOCanvas : MonoBehaviour
{
    Transform cameraJogador;

    void Start()
    {
        cameraJogador = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(this.transform.position + cameraJogador.forward);
    }
}
