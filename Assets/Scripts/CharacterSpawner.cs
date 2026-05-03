using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public Vector3 spawnPosition = new Vector3(-10.36f, -3.27f, 0);
    public GameObject cyborgPrefab;
    public GameObject bikerPrefab;

    void Start()
    {
        GameObject characterToSpawn = null;
        
        // Try to get from CharacterSelector first
        if (CharacterSelector.Instance != null && CharacterSelector.Instance.SelectedCharacter != null)
        {
            characterToSpawn = CharacterSelector.Instance.SelectedCharacter;
        }
        else
        {
            // Fallback to PlayerPrefs
            string selected = PlayerPrefs.GetString("SelectedCharacter", "Cyborg");
            if (selected == "Cyborg" && cyborgPrefab != null)
                characterToSpawn = cyborgPrefab;
            else if (selected == "Biker" && bikerPrefab != null)
                characterToSpawn = bikerPrefab;
        }
        
        if (characterToSpawn != null)
        {
            Instantiate(characterToSpawn, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No character selected. Make sure to select a character in the menu.");
        }
    }
}
