using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDaniable
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Vector2 escalaPersonaje = new Vector2(2f, 2f);
    public EnemySoundController enemySoundController;
    
    //movimiento y deteccion
    public float radioDeteccion = 5f;
    [SerializeField] private float radioAtaque = 1.5f;
    public float velocidadMovimiento = 2f;
   
    private Rigidbody2D rb;
    private float movementX;
    private bool enMovimiento;
    private bool estaPausado;
    private bool atacando = false;
    private Vector2 velocidadGuardada;
    private bool patrullandoHaciaDerecha = true;

    //daño
    private bool recibiendoDanio;
    public float fuerzaRebote = 5f;

    public Animator animator;
    
    //vida
    private bool muerto;
    private bool esperandoChoque;

    public int vida { get; set; } = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if (!muerto && playerController != null && !playerController.muerto)
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
        if (esperandoChoque)
            return;

        Transform player = playerController.transform;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (atacando)
        {
            movementX = 0;
            enMovimiento = false;
        }
        else if (distanceToPlayer < radioAtaque)
        {
            movementX = 0;
            enMovimiento = false;
            enemySoundController.playAtacar();
            atacando = true;
            Vector2 direction = (player.position - transform.position).normalized;
            if (direction.x > 0)
                transform.localScale = new Vector3(-Mathf.Abs(escalaPersonaje.x), escalaPersonaje.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(Mathf.Abs(escalaPersonaje.x), escalaPersonaje.y, transform.localScale.z);
        }
        else if (distanceToPlayer < radioDeteccion)
        {
            
            SeguirJugador(player);
        }
        else
        {
            
            Patrullar();
        }

        if (!recibiendoDanio)
        {
           rb.linearVelocity = new Vector2(movementX * velocidadMovimiento, rb.linearVelocity.y);
        }
    }

    public void DesactivarAtaque()
    {
        atacando = false;
    }

    private void SeguirJugador(Transform player)
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

    private void Patrullar()
    {
        movementX = patrullandoHaciaDerecha ? 1f : -1f;
        
        if (movementX > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(escalaPersonaje.x), escalaPersonaje.y, transform.localScale.z); // Mirar a la derecha
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(escalaPersonaje.x), escalaPersonaje.y, transform.localScale.z); // Mirar a la izquierda
        }
        
        enMovimiento = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!atacando && !recibiendoDanio && !esperandoChoque)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    movementX = 0;
                    enMovimiento = false;
                    rb.linearVelocity = Vector2.zero;
                    esperandoChoque = true;
                    StartCoroutine(EsperarCambiarDireccion());
                    break;
                }
            }
        }
    }

    private IEnumerator EsperarCambiarDireccion()
    {
        yield return new WaitForSeconds(1.2f);
        patrullandoHaciaDerecha = !patrullandoHaciaDerecha;
        esperandoChoque = false;
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

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if(!recibiendoDanio)
        {
            vida -= cantDanio;
            recibiendoDanio = true;

            if (vida <= 0)
            {
                enemySoundController.playMuerte();
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
        if (GameManager.Instance != null)
            GameManager.Instance.SumarPuntos(100);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioAtaque);
    }

}

