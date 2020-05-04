using UnityEngine;
using UnityEngine.Rendering;

public class Personagem : MonoBehaviour
{
    [Header("Movimentação")]
    public float velocidade = 4f;

    [Header("Pulo")]
    public float forcaPulo = 30f;
    public GameObject checarChao;
    public LayerMask layerChao;
    Rigidbody rb;

    [Header("Câmera")]
    public Transform AlvoCamera;
    public float sensibilidade = 2.5f;
    public int rotacaoMaxima = 20;
    float mouseX;
    float mouseY;
    Camera cam;
    Transform obstrucao;
    MeshRenderer obstrucaoRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        obstrucao = AlvoCamera;
    }

    void Update()
    {
        Mover();
        Pular();
        MoverCamera();
        VerPersonagemObstruido();
    }

    void Mover()
    {
        var moverPraFrente = Input.GetAxis("Vertical");
        var moverPraDireita = Input.GetAxis("Horizontal");
        var movimentacao = new Vector3(moverPraDireita, 0, moverPraFrente).normalized * velocidade * Time.deltaTime;
        this.transform.Translate(movimentacao, Space.Self);
    }

    void Pular()
    {
        var estaNoChao = Physics.CheckSphere(checarChao.transform.position, 0.5f, layerChao);

        if (estaNoChao && Input.GetButton("Jump"))
            rb.AddForce(Vector3.up * forcaPulo);
    }

    void MoverCamera()
    {
        mouseX += Input.GetAxis("Mouse X") * sensibilidade;
        mouseY -= Input.GetAxis("Mouse Y") * sensibilidade;
        mouseY = Mathf.Clamp(mouseY, -rotacaoMaxima, rotacaoMaxima);

        cam.transform.LookAt(this.transform);

        AlvoCamera.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        this.transform.rotation = Quaternion.Euler(0, mouseX, 0);
    }

    void VerPersonagemObstruido()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, AlvoCamera.position - cam.transform.position, out hit, 4.5f))
        {
            var distanciaEntreObstrucaoECamera = Vector3.Distance(obstrucao.position, cam.transform.position);
            var distanciaEntreCameraEAlvo = Vector3.Distance(cam.transform.position, AlvoCamera.position);

            if (hit.collider.gameObject.tag != this.tag)
            {
                obstrucao = hit.transform;
                obstrucaoRenderer = obstrucao.gameObject.GetComponent<MeshRenderer>();
                obstrucaoRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;

                if (distanciaEntreObstrucaoECamera >= 3f && distanciaEntreCameraEAlvo >= 1.5f)
                    cam.transform.Translate(Vector3.forward * 1f * Time.deltaTime);
            }
            else
            {
                if (obstrucaoRenderer != null)
                    obstrucaoRenderer.shadowCastingMode = ShadowCastingMode.On;

                if (distanciaEntreCameraEAlvo < 4.5f)
                    cam.transform.Translate(Vector3.back * 1f * Time.deltaTime);
            }
        }
    }
}
