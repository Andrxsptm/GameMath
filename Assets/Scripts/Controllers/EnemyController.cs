using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
     [SerializeField] private Vector2 escalaPersonaje = new Vector2(2f, 2f);
     [SerializeField] private Transform puntoAtaque;
    
    //movimiento y deteccion
    public float radioDeteccion = 5f;
    public float velocidadMovimiento = 2f;
   
    private Rigidbody2D rb;
    private float movementX;
    private bool enMovimiento;
    private bool estaPausado;
    private bool atacando = false;
    private Vector2 velocidadGuardada;

    //daño
    private bool recibiendoDanio;
    public float fuerzaRebote = 5f;

    public Animator animator;
    
    //vida
    private bool muerto;

    public int vida = 3;
    private bool PlayerVivo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerVivo = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        BuscarJugadorActivo();
    }

    void Update()
    {
        if (estaPausado)
            return;

        if (!BuscarJugadorActivo())
        {
            movementX = 0;
            enMovimiento = false;
            animator.SetBool("enMovimiento", false);
            return;
        }
        if (PlayerVivo && !muerto)
        {
            Movimiento();
        }
        animator.SetBool("enMovimiento", enMovimiento);
        animator.SetBool("muerto", muerto);
        animator.SetBool("atacando", atacando);


    }     

    public void Pausar()
    {
        if (estaPausado)
            return;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (rb != null)
        {
            velocidadGuardada = rb.linearVelocity;
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        if (animator != null)
            animator.speed = 0f;

        enMovimiento = false;
        estaPausado = true;
    }

    public void Reanudar()
    {
        if (!estaPausado)
            return;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (rb != null)
        {
            rb.simulated = true;
            rb.linearVelocity = velocidadGuardada;
        }

        if (animator != null)
            animator.speed = 1f;

        estaPausado = false;
    }

    private void Movimiento()
    {
        Transform player = playerController.transform;
         float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < radioDeteccion)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(escalaPersonaje.x), escalaPersonaje.y, transform.localScale.z); // Mirar a la derecha
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Abs(escalaPersonaje.x), escalaPersonaje.y, transform.localScale.z); // Mirar a la izquierda
            }
            movementX = direction.x;
            enMovimiento = true;
        }
        else
        {
            movementX = 0;
            enMovimiento = false;
        }
        if (!recibiendoDanio)
        {
           rb.linearVelocity = new Vector2(movementX * velocidadMovimiento, rb.linearVelocity.y);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerGolpeado = collision.gameObject.GetComponentInParent<PlayerController>();
        if (playerGolpeado != null)
        {
            ActivarAtaque(playerGolpeado);
        }
    } 

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerController>() != null)
        {
            atacando = false;
        }
    }

    private bool BuscarJugadorActivo()
    {
        if (playerController == null)
        {
            // Object.FindObjectOfType is obsolete; use FindFirstObjectByType instead
            playerController = FindAnyObjectByType<PlayerController>();
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

    private void ActivarAtaque(PlayerController playerGolpeado)
    {
        atacando = true;
        Vector2 direccionDanio = new Vector2(transform.position.x, 0);
        playerGolpeado.RecibeDanio(direccionDanio, 1);
        PlayerVivo = !playerGolpeado.muerto;
        if (!PlayerVivo)
        {
            enMovimiento = false;
        }
    }
    
    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if(!recibiendoDanio)
        {
            vida -= cantDanio;
            recibiendoDanio = true;

            if (vida <= 0)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                muerto = true;
                enMovimiento = false;
            }
            else
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse); 
                StartCoroutine(DesactivaDanio());
            }
         
        }
    }
    IEnumerator DesactivaDanio()
    {
        yield return new WaitForSeconds(0.5f); // tiempo de invulnerabilidad después de recibir daño
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero; // para que el enemigo no siga rebotando despues de recibir daño
    }

    private void DestruirEnemigo()
    {
       
        Destroy(gameObject);
        
    }

    // pa dibujar y ajustar el radio de deteccion del enemigo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
    }

}

