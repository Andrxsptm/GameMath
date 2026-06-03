using UnityEngine;

public class ColisionadorAtaque : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            Vector2 direccionDanio = new Vector2(transform.position.x, 0);
            player.RecibeDanio(direccionDanio, 1);
        }
    }
}
