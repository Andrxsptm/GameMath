using UnityEngine;

public class VidaController : MonoBehaviour
{
    public int valorVida = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.RecibirVida(valorVida);
            }
            Destroy(gameObject);
        }
    }
}
