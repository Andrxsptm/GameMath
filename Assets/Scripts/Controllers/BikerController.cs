using UnityEngine;

public class BikerController : MonoBehaviour
{
    GameObject Biker;
    public Animator animator;
    float movement;
    public float movimientoSpeed = 5f;
    Rigidbody2D rb;
    public LayerMask capaPiso;
    public float groundCheckRadius = 0.2f;
    public Transform detectarPiso;
    public bool estaPiso;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");

        if (movement > 0f)
        {
            transform.localScale = new Vector3(4, 4, 4);
        }
        else if (movement < 0f)
        {
            transform.localScale = new Vector3(-4, 4, 4);
        }
        animator.SetFloat("velocidadX", Mathf.Abs(movement));
        //Debug.Log(movement);

        estaPiso = Physics2D.OverlapCircle(
            detectarPiso.position,
            groundCheckRadius,
            capaPiso
            );

        if (Input.GetKeyDown(KeyCode.Space) && estaPiso)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 6f);
        }
        animator.SetBool("piso", estaPiso);
        animator.SetFloat("velocidadY", rb.linearVelocity.y);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement * movimientoSpeed, rb.linearVelocity.y);
    }

}