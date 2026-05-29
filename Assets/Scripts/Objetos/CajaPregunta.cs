using UnityEngine;

public class CajaPregunta : MonoBehaviour
{
    public GameObject ObjectoPregunta;
    private bool destruida = false;

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.CompareTag("GolpeJugador") && !destruida)
        {
            Destruir();
        }
    }
    void Destruir()
    {
        destruida = true;
        Instantiate(ObjectoPregunta, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}