using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector Instance { get; private set; }
    
    public GameObject SelectedCharacter { get; private set; }
    public GameObject cyborgPrefab;
    public GameObject bikerPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Load saved selection
        string selected = PlayerPrefs.GetString("SelectedCharacter", "Cyborg");
        if (selected == "Cyborg" && cyborgPrefab != null)
            SelectedCharacter = cyborgPrefab;
        else if (selected == "Biker" && bikerPrefab != null)
            SelectedCharacter = bikerPrefab;
    }

    public void SelectCyborg()
    {
        SelectedCharacter = cyborgPrefab;
        PlayerPrefs.SetString("SelectedCharacter", "Cyborg");
        Debug.Log("Cyborg selected");
    }

    public void SelectBiker()
    {
        SelectedCharacter = bikerPrefab;
        PlayerPrefs.SetString("SelectedCharacter", "Biker");
        Debug.Log("Biker selected");
    }
}
