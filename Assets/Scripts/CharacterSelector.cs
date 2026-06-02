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
            Instance.personajes.Clear();
            Instance.personajes.AddRange(personajes);
            Instance.CurrentCharacterIndex = Mathf.Clamp(
                PlayerPrefs.GetInt("CharacterIndex", 0),
                0, Instance.personajes.Count - 1);
            Instance.SetPersonaje(Instance.CurrentCharacterIndex);
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
            string orden = "";
            for (int i = 0; i < personajes.Count; i++)
                orden += i + ":" + (personajes[i] != null ? personajes[i].name : "null") + " ";
            Debug.Log("Orden personajes al iniciar: " + orden);
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
        Debug.Log("Personaje seleccionado [" + CurrentCharacterIndex + "]: " + nombrePersonajeActual);
    }

    public void SiguientePersonaje()
    {
        SetPersonaje(CurrentCharacterIndex + 1);
    }

    public void AnteriorPersonaje()
    {
        SetPersonaje(CurrentCharacterIndex - 1);
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
