using UnityEngine;

public class CajaPregunta : MonoBehaviour
{
    public GameObject ObjectoPregunta;
    private bool destruida = false;
    [SerializeField] private AudioClip sonidoDestruccion;

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
        ControladorSonidos.instance.ejecutarSonido(sonidoDestruccion);
        Instantiate(ObjectoPregunta, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}