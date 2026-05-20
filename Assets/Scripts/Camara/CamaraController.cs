using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadCamara = 0.025f;
    public Vector3 desplazamiento;

    void LateUpdate()
    {
        // Si aún no hay objetivo, intenta tomar el personaje instanciado
        if (objetivo == null && CharacterSpawner.PersonajeActual != null)
        {
            objetivo = CharacterSpawner.PersonajeActual.transform;
        }

        if (objetivo == null) return;

        Vector3 posicionDeseada = objetivo.position + desplazamiento;
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);
        transform.position = posicionSuavizada;
    }
}