using UnityEngine;
using System.Collections.Generic;

public class CharacterSpawner : MonoBehaviour
{
    public static GameObject PersonajeActual { get; private set; }

    public Vector3 spawnPosition = new Vector3(-10.36f, -3.27f, 0);
    public List<GameObject> personajesPrefabs = new List<GameObject>();

    void Start()
    {
        int savedIndex = PlayerPrefs.GetInt("CharacterIndex", 0);

        GameObject characterToSpawn = null;

        if (savedIndex >= 0 && savedIndex < personajesPrefabs.Count && personajesPrefabs[savedIndex] != null)
        {
            characterToSpawn = personajesPrefabs[savedIndex];
        }
        else if (personajesPrefabs.Count > 0 && personajesPrefabs[0] != null)
        {
            characterToSpawn = personajesPrefabs[0];
        }

        if (characterToSpawn != null)
        {
            PersonajeActual = Instantiate(characterToSpawn, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No character selected.");
        }
    }
}