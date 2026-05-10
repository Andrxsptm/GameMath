using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class SelectorNivelPersonaje : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelNiveles;
    public GameObject panelPersonajes;

    [Header("Botones Panel Personajes")]
    public Button btn_atras;
    public Button btn_siguiente;
    public Button btn_select;
    public Button btn_close;

    [Header("Vista de personaje")]
    public TextMeshProUGUI textoPersonajeSeleccionado;
    public Image imagenPersonaje; 
    public List<Sprite> sprites = new List<Sprite>();


    [Header("Referencia Transición")]
    public TransicionEscena transicionEscena;

    private string nivelSeleccionado;

    private void Start()
    {
        ConfigurarEstadoInicial();
        ConfigurarBotones();
        ActualizarVistaPersonaje();
    }

    private void ConfigurarEstadoInicial()
    {
        if (panelPersonajes != null)
            panelPersonajes.SetActive(false);

        if (panelNiveles != null)
            panelNiveles.SetActive(true);

    }
    private void ConfigurarBotones()
    {
        if (btn_atras != null)
        {
           
             btn_atras.onClick.AddListener(AnteriorPersonaje);
        }
        if (btn_siguiente != null)
        {
           
            btn_siguiente.onClick.AddListener(SiguientePersonaje);
        }
        if (btn_select != null)
        {
            btn_select.onClick.RemoveAllListeners();
            btn_select.onClick.AddListener(IniciarNivel);
        }
        if (btn_close != null)
        {
            btn_close.onClick.RemoveAllListeners();
            btn_close.onClick.AddListener(IrAtras);
        }
    }

       

    public void SeleccionarNivel(string nombreNivel)
    {
        nivelSeleccionado = nombreNivel;

        // Cambiar de panel
        if (panelNiveles != null)
            panelNiveles.SetActive(false);

        if (panelPersonajes != null)
            panelPersonajes.SetActive(true);

        ActualizarTextoPersonaje();
        Debug.Log("Nivel seleccionado: " + nivelSeleccionado);
    }

    public void IrAtras()
    {
        // Volver al panel de niveles
        if (panelPersonajes != null)
            panelPersonajes.SetActive(false);

        if (panelNiveles != null)
            panelNiveles.SetActive(true);

    }

    public void SiguientePersonaje()
    {
        if (CharacterSelector.Instance != null)
        {
            CharacterSelector.Instance.SiguientePersonaje();
            ActualizarVistaPersonaje();
            Debug.Log("Siguiente personaje");
        }
    }

    public void AnteriorPersonaje()
    {
        if (CharacterSelector.Instance != null)
        {
            CharacterSelector.Instance.AnteriorPersonaje();
            ActualizarVistaPersonaje();
            Debug.Log("Personaje anterior");
        }
    }

    public void IniciarNivel()
    {
        if (string.IsNullOrEmpty(nivelSeleccionado))
        {
            Debug.LogError("No se ha seleccionado ningún nivel");
            return;
        }

        if (CharacterSelector.Instance.SelectedCharacter == null)
        {
            Debug.LogError("No se ha seleccionado ningún personaje");
            return;
        }

        Debug.Log("Iniciando nivel: " + nivelSeleccionado + " con personaje: " + CharacterSelector.Instance.nombrePersonajeActual);

        // Usar transición si existe, sino cargar directamente
        if (transicionEscena != null)
        {
            transicionEscena.CargarEscena(nivelSeleccionado);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nivelSeleccionado);
        }
    }

    private void ActualizarVistaPersonaje()
    {
        if (CharacterSelector.Instance == null)
        {
            Debug.LogError("CharacterSelector no encontrado");
            return;
        }
        ActualizarTextoPersonaje();
        ActualizaeImagenPersonaje();
    }

    private void ActualizarTextoPersonaje()
    {
        if (textoPersonajeSeleccionado == null)
        {
            return;
        }
        textoPersonajeSeleccionado.text = CharacterSelector.Instance.nombrePersonajeActual;
        
    }
    private void ActualizaeImagenPersonaje()
    {
        if (imagenPersonaje == null)
        {
            return;
        }
        int indiceActual = CharacterSelector.Instance.CurrentCharacterIndex;
        if (indiceActual < 0 || indiceActual >= sprites.Count)
        {
            Debug.LogError("Índice de personaje fuera de rango para sprites");
            return;
        }
        imagenPersonaje.sprite = sprites[indiceActual];
    }
    
}