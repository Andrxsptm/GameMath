using UnityEngine;

/// <summary>
/// Helper para probar si el evento OnRespuestaResuelta se dispara correctamente.
/// Puedes agregar este componente a cualquier objeto en la escena para debug.
/// </summary>
public class EventoDebugHelper : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private bool mostrarMensajesDebug = true;

    [Header("Estado")]
    [SerializeField] private bool respuestaCorrectaRecibida = false;
    [SerializeField] private bool respuestaIncorrectaRecibida = false;

    void Start()
    {
        // Buscar PreguntaMatematica y suscribirse al evento
        PreguntaMatematica pm = FindObjectOfType<PreguntaMatematica>();
        if (pm != null)
        {
            pm.OnRespuestaResuelta += ManejarEvento;
        }
        else
        {
            Debug.LogError("[EventoDebugHelper] No se encontro PreguntaMatematica en la escena.");
        }
    }

    private void OnDestroy()
    {
        PreguntaMatematica pm = FindObjectOfType<PreguntaMatematica>();
        if (pm != null)
        {
            pm.OnRespuestaResuelta -= ManejarEvento;
        }
    }

    private void ManejarEvento(bool esCorrecta)
    {
        if (esCorrecta)
        {
            respuestaCorrectaRecibida = true;
            if (mostrarMensajesDebug)
            {
                Debug.Log("[EventoDebugHelper] EVENTO RECIBIDO: Respuesta CORRECTA");
            }
        }
        else
        {
            respuestaIncorrectaRecibida = true;
            if (mostrarMensajesDebug)
            {
                Debug.Log("[EventoDebugHelper] EVENTO RECIBIDO: Respuesta INCORRECTA (sin intentos)");
            }
        }
    }

    [ContextMenu("Reset Estado")]
    public void ResetEstado()
    {
        respuestaCorrectaRecibida = false;
        respuestaIncorrectaRecibida = false;
    }
}
