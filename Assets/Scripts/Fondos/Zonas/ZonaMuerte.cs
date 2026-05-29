using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
          collision.GetComponent<PlayerController>().RecibeDanio(Vector2.zero, 99);
        }
    }

}
