using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public static GameObject PersonajeActual { get; private set; }

    public Vector3 spawnPosition = new Vector3(-10.36f, -3.27f, 0);
    public GameObject cyborgPrefab;
    public GameObject bikerPrefab;

    void Start()
    {
        int savedIndex = PlayerPrefs.GetInt("CharacterIndex", 0);
        string savedName = PlayerPrefs.GetString("SelectedCharacter", "Biker");

        Debug.Log("CharacterSpawner: Index guardado = " + savedIndex + ", Nombre guardado = " + savedName);

        GameObject characterToSpawn = null;

        // Usar índice para decidir
        if (savedIndex == 0 && bikerPrefab != null)
        {
            characterToSpawn = bikerPrefab;
            Debug.Log("CharacterSpawner: Instanciando Biker (índice 0)");
        }
        else if (savedIndex == 1 && cyborgPrefab != null)
        {
            characterToSpawn = cyborgPrefab;
            Debug.Log("CharacterSpawner: Instanciando Cyborg (índice 1)");
        }
        else
        {
            // Fallback: si no coincide ningún índice, usar el primer prefab disponible
            if (bikerPrefab != null)
            {
                characterToSpawn = bikerPrefab;
                Debug.Log("CharacterSpawner: Fallback a Biker");
            }
            else if (cyborgPrefab != null)
            {
                characterToSpawn = cyborgPrefab;
                Debug.Log("CharacterSpawner: Fallback a Cyborg");
            }
        }

        if (characterToSpawn != null)
        {
            PersonajeActual = Instantiate(characterToSpawn, spawnPosition, Quaternion.identity);
            Debug.Log("Personaje instanciado correctamente: " + PersonajeActual.name);
        }
        else
        {
            Debug.LogWarning("No character selected.");
        }
    }
}