using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerSoundControler playerSoundControler;
    bool step1 = false;
    bool fall = false;
    public float timeStep = 0.22f;
    float cont = 0f;
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
    public int vida = 10;

    [Header("Escala del personaje")]
    public Vector3 escalaPersonaje = new Vector3(5f, 5f, 5f);

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
                if (enSuelo && rb.linearVelocity.y < 0 && fall)
                {
                    playerSoundControler.playCaida();
                    fall = false;
                }

                if (enSuelo)
                {
                    dobleSalto = false;
                }

                if (enSuelo && Input.GetKeyDown(KeyCode.Space) && !recibiendoDanio)
                {
                    fall = true;
                    rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
                    playerSoundControler.playSaltar();
                    if (dobleSaltoDesbloqueado)
                    {
                        puedeDobleSaltar = true;
                    }
                }
                else if (!enSuelo && Input.GetKeyDown(KeyCode.Space) && !recibiendoDanio && dobleSaltoDesbloqueado && puedeDobleSaltar)
                {
                    fall = true;
                    dobleSalto = true;
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                    rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
                    puedeDobleSaltar = false;
                    playerSoundControler.playSaltar();
                }
            }

            if (enSuelo && Input.GetKeyDown(KeyCode.C) && !atacando)
            {
                playerSoundControler.playAtacar();
                Atacar();
            }

            if (enSuelo && Input.GetKeyDown(KeyCode.X) && !pateando)
            {
                playerSoundControler.playPatada();
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

        if (inputX != 0 && enSuelo && !recibiendoDanio && !atacando)
        {
            cont += Time.deltaTime;
            if (cont >= timeStep)
            {
                cont = 0f; 
                if (step1)
                {
                    playerSoundControler.playMov1();
                }
                else
                {
                    playerSoundControler.playMov2();
                }
                step1 = !step1;     
            }      
        }

         if (velocidadX > 0)
            {
                transform.localScale = new Vector3(escalaPersonaje.x, escalaPersonaje.y, escalaPersonaje.z);
            }

            if (velocidadX < 0f)
            {
                transform.localScale = new Vector3(-escalaPersonaje.x, escalaPersonaje.y, escalaPersonaje.z);
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
            playerSoundControler.playRecibirDanio();
            recibiendoDanio = true;
            vida -= cantDanio;
            if (vida <= 0)
            {
                playerSoundControler.playMuerte();
                muerto = true;
            }
            if (!muerto)
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.5f).normalized;
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
            }
        }
    }

    public void MostrarGameOver()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
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
        playerSoundControler.playPatada();
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
