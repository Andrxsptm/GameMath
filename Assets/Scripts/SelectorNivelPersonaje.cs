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

    [Header("Texto (opcional)")]
    public TextMeshProUGUI textoPersonajeActual;
    public List<Sprite> sprites = new List<Sprite>();

    [Header("Referencia Transición")]
    public TransicionEscena transicionEscena;

    private string nivelSeleccionado;

    private void Start()
    {
        // Configurar estado inicial
        if (panelPersonajes != null)
            panelPersonajes.SetActive(false);

        if (panelNiveles != null)
            panelNiveles.SetActive(true);

        // Asignar funciones a los botones del panel de personajes
        // btn_atras = PreviousPersonaje (retroceder personaje)
        if (btn_atras != null)
            btn_atras.onClick.AddListener(PreviousPersonaje);

        // btn_siguiente = NextPersonaje (siguiente personaje)
        if (btn_siguiente != null)
            btn_siguiente.onClick.AddListener(SiguientePersonaje);

        if (btn_select != null)
            btn_select.onClick.AddListener(IniciarNivel);

        // btn_close = Volver al panel de niveles
        if (btn_close != null)
            btn_close.onClick.AddListener(IrAtras);

        // Asegurar que el botón next esté habilitado
        if (btn_siguiente != null)
            btn_siguiente.interactable = true;

        // Actualizar texto inicial
        ActualizarTextoPersonaje();
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

        Debug.Log("Volviendo a selección de nivel");
    }

    public void SiguientePersonaje()
    {
        if (CharacterSelector.Instance != null)
        {
            CharacterSelector.Instance.SiguientePersonaje();
            ActualizarTextoPersonaje();
            Debug.Log("Siguiente personaje");
        }
    }

    public void PreviousPersonaje()
    {
        if (CharacterSelector.Instance != null)
        {
            CharacterSelector.Instance.PreviousPersonaje();
            ActualizarTextoPersonaje();
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

    private void ActualizarTextoPersonaje()
    {
        if (textoPersonajeActual != null && CharacterSelector.Instance != null)
        {
            textoPersonajeActual.text = CharacterSelector.Instance.nombrePersonajeActual;
        }
    }
    
}