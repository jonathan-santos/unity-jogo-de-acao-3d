using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject[] objetos;
    public bool iniciarAutomaticamente = true;
    public float intervaloDeSpawn;
    public bool terminouDeSpawnar = false;

    [Header("Quantidade")]
    public int quantidadeMinima;
    public int quantidadeMaxima;

    [Header("Distância")]
    public float distanciaMinimaDoSpawn = 10;
    public float distanciaMaximaDoSpawn = 30;

    GameObject objetoRandomico;
    int contagemMaxima;
    int contagem = 0;

    void Start()
    {
        if (this.iniciarAutomaticamente)
            StartSpawn();
    }

    public void StartSpawn()
    {
        this.terminouDeSpawnar = false;

        contagem = 0;
        contagemMaxima = Random.Range(quantidadeMinima, quantidadeMaxima);
        if (intervaloDeSpawn == 0)
        {
            for (int i = 0; i < contagemMaxima; i++)
            {
                Spawn();
            }
            this.terminouDeSpawnar = true;
        }
        else
        {
            InvokeRepeating("Spawn", 0, intervaloDeSpawn);
        }
    }

    void Update()
    {
        if (contagem >= contagemMaxima)
        {
            this.terminouDeSpawnar = true;
            CancelInvoke();
        }
    }

    void Spawn()
    {
        int index = Random.Range(0, objetos.Length);

        objetoRandomico = Instantiate(objetos[index]);

        objetoRandomico.transform.parent = transform;

        objetoRandomico.transform.localPosition = new Vector3(
            Random.Range((transform.position.x + distanciaMinimaDoSpawn) - distanciaMaximaDoSpawn, transform.position.x + distanciaMaximaDoSpawn),
            0,
            Random.Range((transform.position.z + distanciaMinimaDoSpawn) - distanciaMaximaDoSpawn, transform.position.z + distanciaMaximaDoSpawn)
        );

        contagem++;
    }
}
