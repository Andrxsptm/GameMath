using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float velocidad = 5;
    
    public Animator animator;
    //salto
    public float fuerzaSalto = 10f;
    public float longitudRaycast = 0.1f;
    public LayerMask capaSuelo;
    private bool enSuelo;
    private Rigidbody2D rb;
    private bool estaPausado;
    private Vector2 velocidadGuardada;

    //Daño 
    private bool recibiendoDanio;
    public float fuerzaRebote = 5f;

    // Atacar
    private bool atacando;
    private bool pateando;
    public bool muerto;

    
    //vidas
    public int vida = 3;

    [Header ("Doble salto")]
    public bool dobleSaltoDesbloqueado = false;
    private bool puedeDobleSaltar = false;
    private bool dobleSalto = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (estaPausado)
            return;

        if (!muerto)
        {
            if (!atacando)
            {
                Movimiento();
                // Pa no usar otro objeto y detectar el suelo
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
                enSuelo = hit.collider != null; // si la linea colisiona con el suelo sera verdadero

                if (enSuelo)
                {
                    dobleSalto = false;
                }

                if (enSuelo && Input.GetKeyDown(KeyCode.Space) && !recibiendoDanio)
                {
                    rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
                    if (dobleSaltoDesbloqueado)
                    {
                        puedeDobleSaltar = true;
                    }
                }
                else if (!enSuelo && Input.GetKeyDown(KeyCode.Space) && !recibiendoDanio && dobleSaltoDesbloqueado && puedeDobleSaltar)
                {
                    dobleSalto = true;
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                    rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
                    puedeDobleSaltar = false;
                }
            }

            if (enSuelo && Input.GetKeyDown(KeyCode.C) && !atacando)
            {
                Atacar();
            }

            if (enSuelo && Input.GetKeyDown(KeyCode.X) && !pateando)
            {
                Patada();
            }
        }

        animator.SetBool("ensuelo", enSuelo);
        animator.SetBool("recibeDanio", recibiendoDanio);
        animator.SetBool("Atacando", atacando);
        animator.SetBool("muerto", muerto);
        animator.SetBool("dobleSalto", dobleSalto);
        animator.SetBool("pateando", pateando);
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

    public void Movimiento()
    {
        float inputX = Input.GetAxis("Horizontal");
        float velocidadX = inputX * Time.deltaTime * velocidad;
        animator.SetFloat("movement", velocidadX);

        if (velocidadX > 0)
        {
            transform.localScale = new Vector3(5, 5, 5);
        }

        if (velocidadX < 0f)
        {
            transform.localScale = new Vector3(-5, 5, 5);
        }

        Vector3 posicion = transform.position;

        if (!recibiendoDanio)
        {
            transform.position = new Vector3(velocidadX + posicion.x, posicion.y, posicion.z);
        }
    }

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDanio)
        {
            recibiendoDanio = true;
            vida -= cantDanio;
            if (vida <= 0)
            {
                muerto = true;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }
            }
            if (!muerto)
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.5f).normalized;
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
            }
        }
    }

    public void RecibirVida(int cantidad)
    {
        vida += cantidad;
    }

    public void DesactivaDanio()
    {
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero; // para que el personaje no siga rebotando despues de recibir daño
    }

    public void Atacar()
    {
        atacando = true;
    }

    public void DesactivaAtaque()
    {
        atacando = false;
    }

    public void Patada()
    {
        pateando = true;
    }

    public void DesactivaPatada()
    {
        pateando = false;
    }

    void OnDrawGizmos() // pa dibujar el colisionador del raycast
    {
	Gizmos.color = Color.red;
	Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
    
}
