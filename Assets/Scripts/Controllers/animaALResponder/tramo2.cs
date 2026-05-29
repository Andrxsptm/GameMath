using UnityEngine;
using System.Collections;

public class tramo2 : MonoBehaviour
{
    [Header("Referencia al sistema de preguntas")]
    [SerializeField] private PreguntaMatematica preguntaMatematica;

    [Header("Referencia al tramo1")]
    [Tooltip("Arrastra aqui el componente tramo1 del objeto tramo1")]
    public tramo1 tramo1Script;

    [Header("Animator")]
    [Tooltip("Animator del objeto a animar")]
    public Animator animator;

    [Header("Configuracion")]
    [Tooltip("Nombre de la animacion a reproducir")]
    public string nombreAnimacion = "baja";

    [Tooltip("Duracion aproximada de la animacion (segundos)")]
    public float duracionAnimacion = 2f;

    [Header("Hijos")]
    [Tooltip("Nombre del trigger para los Animators de los hijos")]
    public string nombreAnimacionHijos = "quieto";

    private Animator[] animatorsHijos;
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

        animatorsHijos = GetComponentsInChildren<Animator>(true);

        // Suscribirse al evento de respuesta
        if (preguntaMatematica != null)
        {
            preguntaMatematica.OnRespuestaResuelta += ManejarRespuesta;
        }
        else
        {
            Debug.LogError("[tramo2] No se encontro PreguntaMatematica. Asegurate de que exista en la escena.");
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
        // tramo2 verifica si debe activarse cuando llega la respuesta
        if (esCorrecta && tramo1Script != null)
        {
            // Verificamos si tramo1 esta desactivado (ya termino)
            if (!tramo1Script.enabled && !animacionIniciada)
            {
                Debug.Log("[tramo2] Respuesta correcta y tramo1 desactivado. Iniciando animacion.");
                animacionIniciada = true;
                StartCoroutine(IniciarAnimacionTramo2());
            }
        }
    }

    private IEnumerator IniciarAnimacionTramo2()
    {
        Debug.Log("[tramo2] Iniciando animacion: " + nombreAnimacion);

        // Reproducir la animacion en el padre
        if (animator != null)
        {
            animator.SetTrigger(nombreAnimacion);
        }
        else
        {
            Debug.LogError("[tramo2] No se encontro Animator en este objeto.");
        }

        // Reproducir la animacion en los hijos
        foreach (Animator childAnimator in animatorsHijos)
        {
            if (childAnimator != null && childAnimator != animator)
            {
                childAnimator.SetTrigger(nombreAnimacionHijos);
            }
        }

        // Esperar a que la animacion termine
        yield return new WaitForSeconds(duracionAnimacion);

        Debug.Log("[tramo2] Animacion completada.");
    }
}
