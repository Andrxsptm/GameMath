using UnityEngine;

public class ParallaxController : MonoBehaviour
{
  Transform cam; //Main Camera
    Vector3 camStartPos;
    float distance; //jarak antara start camera posisi dan current posisi

    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthestBack;

    [Range(0.01f, 1f)]
    public float parallaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            Renderer renderer = backgrounds[i].GetComponent<Renderer>();
            if (renderer != null)
                mat[i] = renderer.material;
        }

        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            if (backgrounds[i] == null) continue;
            float dist = backgrounds[i].transform.position.z - cam.position.z;
            if (dist > farthestBack)
            {
                farthestBack = dist;
            }
        }

        for (int i = 0; i < backCount; i++)
        {
            if (backgrounds[i] == null) continue;
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
        }
    }

    private void LateUpdate()
    {
        distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x - 1, transform.position.y, 2.8f);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (mat[i] == null) continue;
            float speed = backSpeed[i] * parallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
