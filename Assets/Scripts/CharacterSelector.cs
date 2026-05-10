using UnityEngine;
using System.Collections.Generic;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector Instance { get; private set; }
    
    public GameObject SelectedCharacter { get; private set; }
    public int CurrentCharacterIndex { get; private set; }
    
    public List<GameObject> personajes = new List<GameObject>();
    
    [HideInInspector] public string nombrePersonajeActual;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Cargar índice guardado o usar 0 por defecto
        CurrentCharacterIndex = PlayerPrefs.GetInt("CharacterIndex", 0);
        
        // Asegurar que el índice sea válido
        if (personajes.Count > 0)
        {
            CurrentCharacterIndex = Mathf.Clamp(CurrentCharacterIndex, 0, personajes.Count - 1);
            SetPersonaje(CurrentCharacterIndex);
        }
    }

    public void SetPersonaje(int index)
    {
        if (personajes.Count == 0) return;
        
        CurrentCharacterIndex = Mathf.Clamp(index, 0, personajes.Count - 1);
        SelectedCharacter = personajes[CurrentCharacterIndex];
        
        PlayerPrefs.SetInt("CharacterIndex", CurrentCharacterIndex);
        nombrePersonajeActual = SelectedCharacter.name;
        Debug.Log("Personaje seleccionado: " + nombrePersonajeActual);
    }

    public void SiguientePersonaje()
    {
        int nuevoIndex = (CurrentCharacterIndex + 1 + personajes.Count) % personajes.Count;
        SetPersonaje(nuevoIndex);
    }

    public void AnteriorPersonaje()
    {
       int nuevoIndex = (CurrentCharacterIndex - 1 ) % personajes.Count;
        SetPersonaje(nuevoIndex);
    }

    public void SelectByName(string nombre)
    {
        for (int i = 0; i < personajes.Count; i++)
        {
            if (personajes[i].name == nombre)
            {
                SetPersonaje(i);
                return;
            }
        }
    }

    public int GetTotalPersonajes()
    {
        return personajes.Count;
    }
}
