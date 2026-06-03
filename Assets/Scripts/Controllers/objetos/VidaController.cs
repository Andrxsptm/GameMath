using UnityEngine;

public class VidaController : MonoBehaviour
{
    public int valorVida = 1;
    [SerializeField] private AudioClip sonidoVida;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                ControladorSonidos.instance.ejecutarSonido(sonidoVida);
                player.RecibirVida(valorVida);
            }
            Destroy(gameObject);
        }
    }
}
