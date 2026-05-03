using UnityEngine;

public class AutoParallax : MonoBehaviour
{
    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthestBack = 0.01f;
    float initialZ;

    [Header("Configuración de Velocidad")]
    [Tooltip("Velocidad general del movimiento")]
    public float scrollSpeed = 0.5f;

    [Tooltip("Dirección del movimiento: 1 para izquierda, -1 para derecha")]
    public float direction = 1f;

    // Guardamos el desplazamiento acumulado a lo largo del tiempo
    private float offset;

    void Start()
    {
        initialZ = transform.position.z;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        // 1. Encontrar el objeto más alejado en el eje Z respecto al padre
        for (int i = 0; i < backCount; i++)
        {
            float distanceZ = backgrounds[i].transform.position.z - transform.position.z;
            if (distanceZ > farthestBack)
            {
                farthestBack = distanceZ;
            }
        }

        // 2. Calcular la velocidad individual de cada capa
        for (int i = 0; i < backCount; i++)
        {
            float distanceZ = backgrounds[i].transform.position.z - transform.position.z;

            if (farthestBack != 0)
            {
                // Las capas más lejanas se moverán más lento que las cercanas
                backSpeed[i] = 1 - (distanceZ / farthestBack);
            }
            else
            {
                backSpeed[i] = 1;
            }
        }
    }

    private void Update()
    {
        // Acumulamos el movimiento basándonos en el tiempo real (independiente de los FPS)
        offset += Time.deltaTime * scrollSpeed * direction;

        // Aplicamos el desplazamiento a las texturas de cada capa
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // Cada capa multiplica el offset global por su propia velocidad asignada
            float layerOffset = offset * backSpeed[i];
            mat[i].SetTextureOffset("_MainTex", new Vector2(layerOffset, 0));
        }
    }
}