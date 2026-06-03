using UnityEngine;

public class MonedaController : MonoBehaviour
{
    public int valorMoneda = 1;
    [SerializeField] private AudioClip sonidoMoneda;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.SumarMoneda(valorMoneda);
            ControladorSonidos.instance.ejecutarSonido(sonidoMoneda);
            Destroy(gameObject);
        }
    }
}
