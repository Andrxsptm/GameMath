using UnityEngine;

public class MonedaController : MonoBehaviour
{
    public int valorMoneda = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.SumarMoneda(valorMoneda);
            Destroy(gameObject);
        }
    }
}
