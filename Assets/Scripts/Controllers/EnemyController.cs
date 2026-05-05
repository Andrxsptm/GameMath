using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    public float detectionRadius = 5f;
    public float speed = 2f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool enMovimiento;
    private bool recibiendoDanio;
    public float fuerzaRebote = 5f;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        BuscarJugadorActivo();
    }

    void Update()
    {
        if (!BuscarJugadorActivo())
        {
            movement = Vector2.zero;
            enMovimiento = false;
            animator.SetBool("enMovimiento", false);
            return;
        }

        Transform player = playerController.transform;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(-2, 2, 2); // Mirar a la derecha
            }
            else
            {
                transform.localScale = new Vector3(2, 2, 2); // Mirar a la izquierda
            }
            movement = new Vector2(direction.x, 0);
            enMovimiento = true;
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }
        if (!recibiendoDanio)
        {
           rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }

        animator.SetBool("enMovimiento", enMovimiento);

    }     
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerGolpeado = collision.gameObject.GetComponent<PlayerController>();
        if (playerGolpeado != null)
        {
            Vector2 direccionDanio = new Vector2(transform.position.x, 0);
            playerGolpeado.RecibeDanio(direccionDanio, 1);
        }
    } 

    private bool BuscarJugadorActivo()
    {
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        return playerController != null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GolpeJugador"))
        {
            Vector2 direccionDanio = new Vector2(collision.gameObject.transform.position.x, 0);
            RecibeDanio(direccionDanio, 1);
        }
    }
    
    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if(!recibiendoDanio)
        {
        recibiendoDanio = true;
        Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
        rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse); 
        StartCoroutine(DesactivaDanio());
        }
    }
    IEnumerator DesactivaDanio()
    {
        yield return new WaitForSeconds(0.5f); // tiempo de invulnerabilidad después de recibir daño
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero; // para que el enemigo no siga rebotando despues de recibir daño
    }

    // pa dibujar y ajustar el radio de deteccion del enemigo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}

