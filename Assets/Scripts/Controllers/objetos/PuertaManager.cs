using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaManager : MonoBehaviour
{
    [Header("Referencia a PreguntaMatematica")]
    [Tooltip("Asigna aquí el componente PreguntaMatematica de la escena.")]
    public PreguntaMatematica preguntaMatematica;

    [Header("Configuración de Penalización")]
    [Tooltip("Daño que recibirá el jugador si falla la pregunta.")]
    public int danioPorFallo = 1;

    private bool jugadorEnPuerta = false;
    private bool preguntaActiva = false;
    private PlayerController jugador;
    private bool puertaResuelta = false;

    private void Start()
    {
        // Intentar buscar automáticamente si no se asignó
        if (preguntaMatematica == null)
        {
            preguntaMatematica = FindObjectOfType<PreguntaMatematica>();
            if (preguntaMatematica == null)
            {
                Debug.LogError("[PuertaManager] No se encontró PreguntaMatematica en la escena. Por favor, asígnele en el Inspector.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (puertaResuelta || preguntaActiva)
            return;

        jugador = collision.GetComponentInParent<PlayerController>();

        // Si no encontramos el PlayerController directamente, intentamos obtenerlo del objeto colisionado
        if (jugador == null)
        {
            jugador = collision.GetComponent<PlayerController>();
        }

        if (jugador != null && !jugador.muerto)
        {
            jugadorEnPuerta = true;
            AbrirPregunta();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        jugadorEnPuerta = false;
    }

    private void AbrirPregunta()
    {
        if (preguntaMatematica == null)
        {
            Debug.LogError("[PuertaManager] No hay referencia a PreguntaMatematica. No se puede abrir la pregunta.");
            return;
        }

        preguntaActiva = true;

        // Pausar al jugador mientras responde
        jugador.Pausar();

        // Abrir el panel de pregunta
        preguntaMatematica.AbrirPanel(jugador);

        // Suscribirse al evento de resolución de la pregunta
        preguntaMatematica.OnRespuestaResuelta += ManejarResultadoPregunta;
    }

    private void ManejarResultadoPregunta(bool esCorrecta)
    {
        // Desuscribirse del evento para evitar múltiples llamadas
        if (preguntaMatematica != null)
        {
            preguntaMatematica.OnRespuestaResuelta -= ManejarResultadoPregunta;
        }

        if (esCorrecta)
        {
            // Respuesta correcta: desbloquear y abrir el panel de victoria
            puertaResuelta = true;
            StartCoroutine(AbrirPanelWin());
        }
        else
        {
            // Respuesta incorrecta: penalizar al jugador y reiniciar nivel
            AplicarPenalizacion();
        }

        preguntaActiva = false;
    }

    private IEnumerator AbrirPanelWin()
    {
        yield return new WaitForSeconds(0.5f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AbrirPanelWin();
        }
        else
        {
            Debug.LogError("[PuertaManager] No se encontró GameManager.Instance. No se puede abrir panelWin.");
        }
    }

    private void AplicarPenalizacion()
    {
        if (jugador != null)
        {
            jugador.vida -= danioPorFallo;

            // Reanudar al jugador para que vea el daño
            jugador.Reanudar();

            if (jugador.vida <= 0)
            {
                jugador.muerto = true;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }
                return;
            }
        }

        // Reiniciar la escena actual
        StartCoroutine(ReiniciarEscenaActual());
    }

    private IEnumerator ReiniciarEscenaActual()
    {
        // Pequeña espera para que se vea el cierre del panel
        yield return new WaitForSeconds(0.5f);

        // Reiniciar la escena actual
        string escenaActual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(escenaActual);
    }

    private void OnDisable()
    {
        // Asegurarse de desuscribirse del evento si el objeto se desactiva/destruye
        if (preguntaMatematica != null)
        {
            preguntaMatematica.OnRespuestaResuelta -= ManejarResultadoPregunta;
        }
    }
}
