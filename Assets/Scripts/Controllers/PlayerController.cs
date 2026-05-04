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

    //Daño 
    private bool recibiendoDanio;
    public float fuerzaRebote = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float velocidadX = inputX * Time.deltaTime * velocidad;
        animator.SetFloat("movement", velocidadX);
	    if (velocidadX > 0)
        {
            transform.localScale = new Vector3(4, 4, 4);
        }
        if (velocidadX < 0f)
        {
            transform.localScale = new Vector3(-4, 4, 4);
        }
	    Vector3 posicion = transform.position;
        
        if(!recibiendoDanio)
        {
            transform.position = new Vector3(velocidadX + posicion.x, posicion.y, posicion.z);
        }
	    
        // Pa no usar otro objeto y detectar el suelo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        
	enSuelo = hit.collider != null; // si la linea colisiona con el suelo sera verdadero

        if (enSuelo && Input.GetKeyDown(KeyCode.Space) && !recibiendoDanio)
        {
            rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
        }
	animator.SetBool("ensuelo", enSuelo);
	animator.SetBool("recibeDanio", recibiendoDanio);
    }

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
	if(!recibiendoDanio)
	{
	   recibiendoDanio = true;
	   Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized;
	   rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse); 
	}
	
    }
    public void DesactivaDanio()
    {
	    recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero; // para que el personaje no siga rebotando despues de recibir daño
    }
    void OnDrawGizmos() // pa dibujar el colisionador del raycast
    {
	Gizmos.color = Color.red;
	Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}
