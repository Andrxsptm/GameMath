using UnityEngine;

public class PreguntaController : MonoBehaviour
{
    [Header("Referencia al Panel de Preguntas")]
    [SerializeField] private PreguntaMatematica preguntaMatematica;
    [SerializeField] private AudioClip sonidoPregunta;

    [Header("Configuracion")]
    [Tooltip("Tiempo de espera (segundos) para que el objeto pueda verse y tocarse de nuevo si falla la pregunta")]
    public float tiempoEsperaReactivacion = 2f;

    private bool preguntaResuelta = false;
    private bool eventoSuscrito = false;
    private bool escondido = false;

    // Cacheamos componentes para mejor performance
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private void Awake()
    {
        BuscarPreguntaMatematica();
        ObtenerComponentesVisuales();
    }

    private void OnEnable()
    {
        BuscarPreguntaMatematica();
        SuscribirseAlEvento();
    }

    private void OnDisable()
    {
        DesuscribirseDelEvento();
    }

    private void ObtenerComponentesVisuales()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (col == null)
            col = GetComponent<Collider2D>();
    }

    private void BuscarPreguntaMatematica()
    {
        if (preguntaMatematica != null) return;

        preguntaMatematica = FindObjectOfType<PreguntaMatematica>();

        if (preguntaMatematica == null)
        {
            GameObject pmObj = GameObject.Find("PreguntaMatematica");
            if (pmObj != null)
            {
                preguntaMatematica = pmObj.GetComponent<PreguntaMatematica>();
            }
        }

        if (preguntaMatematica == null)
        {
            Debug.LogError("[PreguntaController] No se encontro PreguntaMatematica en la escena.");
        }
    }

    private void SuscribirseAlEvento()
    {
        if (preguntaMatematica != null && !eventoSuscrito)
        {
            preguntaMatematica.OnRespuestaResuelta += ManejarResultadoRespuesta;
            eventoSuscrito = true;
        }
    }

    private void DesuscribirseDelEvento()
    {
        if (preguntaMatematica != null && eventoSuscrito)
        {
            preguntaMatematica.OnRespuestaResuelta -= ManejarResultadoRespuesta;
            eventoSuscrito = false;
        }
    }

    /// <summary>
    /// Esconde el objeto visualmente (transparente y sin colision) pero mantiene el script activo.
    /// </summary>
    private void EsconderObjeto()
    {
        if (escondido) return;
        escondido = true;

        ObtenerComponentesVisuales();

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        if (col != null)
            col.enabled = false;

        Debug.Log("[PreguntaController] Objeto escondido mientras la pregunta esta activa.");
    }

    /// <summary>
    /// Muestra el objeto de nuevo para que se pueda tocar.
    /// </summary>
    private void MostrarObjeto()
    {
        escondido = false;

        ObtenerComponentesVisuales();

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
        if (col != null)
            col.enabled = true;

        Debug.Log("[PreguntaController] Objeto mostrado de nuevo.");
    }

    private void ManejarResultadoRespuesta(bool esCorrecta)
    {
        if (preguntaResuelta) return;

        if (esCorrecta)
        {
            preguntaResuelta = true;
            Debug.Log("[PreguntaController] Respuesta correcta. Destruyendo objeto.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("[PreguntaController] Respuesta incorrecta. Esperando " + tiempoEsperaReactivacion + " segundos para mostrar de nuevo.");
            ControladorSonidos.instance.ejecutarSonido(sonidoPregunta);
            // Usamos un Invoke que SIEMPRE funcionara porque el script SIEMPRE esta activo
            Invoke(nameof(MostrarObjeto), tiempoEsperaReactivacion);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (escondido || preguntaResuelta) return;

        BuscarPreguntaMatematica();

        if (preguntaMatematica == null)
            return;

        PlayerController jugador = collision.GetComponentInParent<PlayerController>();

        if (jugador != null)
        {
            // En lugar de desactivar el objeto, solo lo escondemos visualmente
            ControladorSonidos.instance.ejecutarSonido(sonidoPregunta);
            EsconderObjeto();
            preguntaMatematica.AbrirPanel(jugador);
        }
    }
}
