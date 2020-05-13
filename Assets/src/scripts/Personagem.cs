using UnityEngine;
using UnityEngine.Rendering;

public class Personagem : MonoBehaviour
{
    [Header("Movimentação")]
    public float velocidadeMovimentacao = 5f;
    public float velocidadeGirarCorpo = 0.2f;
    public Transform corpo;
    Vector3 direcaoMovimento;

    [Header("Pulo")]
    public float alturaPulo = 30f;
    public float forcaPulo = 25f;
    public GameObject checarChao;
    public LayerMask layerChao;
    bool estaTocandoNoChao;
    Rigidbody rb;

    [Header("Câmera")]
    public Transform alvoCamera;
    public float sensibilidade = 2.5f;
    public int rotacaoMaxima = 20;
    float mouseX;
    float mouseY;
    Camera cam;
    Transform obstrucao;
    MeshRenderer obstrucaoRenderer;

    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        anim = GetComponent<Animator>();

        obstrucao = alvoCamera;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        estaTocandoNoChao = Physics.CheckSphere(checarChao.transform.position, 0.5f, layerChao);

        Mover();
        Pular();
        Atacar();
    }

    void FixedUpdate()
    {
        MoverCamera();
        GirarCorpoPersonagem();
    }

    void Mover()
    {
        var velocidade = velocidadeMovimentacao;

        if (Input.GetButton("Fire3"))
            velocidade *= 1.25f;                 

        if (!estaTocandoNoChao)
            velocidade *= 0.5f;

        var moverPraFrente = Input.GetAxis("Vertical");
        var moverPraDireita = Input.GetAxis("Horizontal");
        direcaoMovimento = new Vector3(moverPraDireita, 0, moverPraFrente).normalized;
        transform.Translate(direcaoMovimento * velocidade * Time.deltaTime, alvoCamera);
    }

    void Pular()
    {
        if (estaTocandoNoChao && Input.GetButton("Jump"))
        {
            var quantoIrPraCima = Vector3.up * alturaPulo;
            var direcaoPulo = alvoCamera.TransformDirection(direcaoMovimento) * forcaPulo;
            rb.AddForce(quantoIrPraCima + direcaoPulo, ForceMode.Force);
        }
    }

    void MoverCamera()
    {
        mouseX += Input.GetAxis("Mouse X") * sensibilidade;
        mouseY -= Input.GetAxis("Mouse Y") * sensibilidade;
        mouseY = Mathf.Clamp(mouseY, -rotacaoMaxima, rotacaoMaxima);

        alvoCamera.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        cam.transform.LookAt(this.transform);
    }

    void GirarCorpoPersonagem()
    {
        if (direcaoMovimento != Vector3.zero)
        {
            corpo.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);
            if ((Input.GetAxis("Mouse X") == 0))
                corpo.rotation = Quaternion.LookRotation(corpo.TransformDirection(direcaoMovimento));
        }
    }

    void VerPersonagemObstruido()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, alvoCamera.position - cam.transform.position, out hit, 4.5f))
        {
            var distanciaEntreObstrucaoECamera = Vector3.Distance(obstrucao.position, cam.transform.position);
            var distanciaEntreCameraEAlvo = Vector3.Distance(cam.transform.position, alvoCamera.position);

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

    void Atacar()
    {
        if (Input.GetButtonDown("Fire1"))
            anim.SetTrigger("atacar");
    }
}