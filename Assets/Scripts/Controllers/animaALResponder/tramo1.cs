using UnityEngine;
using System.Collections;

public class tramo1 : MonoBehaviour
{
    [Header("Referencia al sistema de preguntas")]
    [SerializeField] private PreguntaMatematica preguntaMatematica;

    [Header("Animator")]
    [Tooltip("Animator del objeto a animar")]
    public Animator animator;

    [Header("Configuracion")]
    [Tooltip("Nombre de la animacion a reproducir")]
    public string nombreAnimacion = "baja";

    [Tooltip("Duracion aproximada de la animacion (segundos). Se usara para desactivar el script despues")]
    public float duracionAnimacion = 2f;

    private bool animacionIniciada = false;

    void Start()
    {
        if (preguntaMatematica == null)
        {
            BuscarPreguntaMatematica();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Suscribirse al evento de respuesta
        if (preguntaMatematica != null)
        {
            preguntaMatematica.OnRespuestaResuelta += ManejarRespuesta;
        }
        else
        {
            Debug.LogError("[tramo1] No se encontro PreguntaMatematica. Asegurate de que exista en la escena.");
        }
    }

    private void BuscarPreguntaMatematica()
    {
        // Buscar por tipo
        preguntaMatematica = FindObjectOfType<PreguntaMatematica>();
        
        // Si no se encontro, intentar por nombre
        if (preguntaMatematica == null)
        {
            GameObject pmObj = GameObject.Find("PreguntaMatematica");
            if (pmObj != null)
            {
                preguntaMatematica = pmObj.GetComponent<PreguntaMatematica>();
            }
        }
    }

    private void OnDestroy()
    {
        if (preguntaMatematica != null)
        {
            preguntaMatematica.OnRespuestaResuelta -= ManejarRespuesta;
        }
    }

    private void ManejarRespuesta(bool esCorrecta)
    {
        if (!esCorrecta || animacionIniciada)
            return;

        animacionIniciada = true;
        StartCoroutine(IniciarAnimacionTramo1());
    }

    private IEnumerator IniciarAnimacionTramo1()
    {
        Debug.Log("[tramo1] Iniciando animacion: " + nombreAnimacion);

        // Reproducir la animacion
        if (animator != null)
        {
            animator.SetTrigger(nombreAnimacion);
        }
        else
        {
            Debug.LogError("[tramo1] No se encontro Animator en este objeto.");
        }

        // Esperar a que la animacion termine
        yield return new WaitForSeconds(duracionAnimacion);

        Debug.Log("[tramo1] Animacion completada. Desactivando script.");

        // Desactivar este script para que ya no funcione mas
        enabled = false;
    }
}
