using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Collections;

public class PreguntaMatematica : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelPregunta;
    public TextMeshProUGUI textoPregunta;
    public TextMeshProUGUI textoIntentos;
    public TextMeshProUGUI textoFeedback;
    public TMP_InputField inputRespuesta;

    [Header("Configuracion")]
    public string siguienteNivel = "Lvl2";
    public int intentosMaximos = 3;

    public enum TipoOperacion { Suma, Resta, Multiplicacion, Division, Todas }
    public TipoOperacion tipoOperacion = TipoOperacion.Todas;

    // Evento que notifica cuando se resuelve la pregunta (true = correcta, false = falló)
    public event Action<bool> OnRespuestaResuelta;

    private int respuestaCorrecta;
    public bool respuestaValida = false;
    private int intentosRestantes;
    private bool panelAbierto = false;
    private PlayerController jugadorActual;

    void Start()
    {
        panelPregunta.SetActive(false);
        intentosRestantes = intentosMaximos;
    }

    private PlayerController ObtenerJugador()
    {
        if (jugadorActual != null)
            return jugadorActual;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            jugadorActual = FindObjectOfType<PlayerController>();
            return jugadorActual;
        }

        jugadorActual = player.GetComponent<PlayerController>();
        if (jugadorActual == null)
            jugadorActual = player.GetComponentInParent<PlayerController>();

        if (jugadorActual == null)
            jugadorActual = FindObjectOfType<PlayerController>();

        return jugadorActual;
    }

    void PausarEnemigos(bool pausar)
    {
        EnemyController[] enemigos = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemigo in enemigos)
        {
            if (enemigo == null)
                continue;

            if (pausar)
                enemigo.Pausar();
            else
                enemigo.Reanudar();
        }
    }

    void PausarJugador(bool pausar)
    {
        PlayerController jugador = ObtenerJugador();
        if (jugador == null)
            return;

        if (pausar)
            jugador.Pausar();
        else
            jugador.Reanudar();
    }

    public void AbrirPanel()
    {
        AbrirPanel(null);
    }

    public void AbrirPanel(PlayerController jugador)
    {
        if (panelAbierto)
            return;

        panelAbierto = true;
        jugadorActual = jugador;
        intentosRestantes = intentosMaximos;
        GenerarPregunta();

        PausarJugador(true);
        PausarEnemigos(true);

        panelPregunta.SetActive(true);

        inputRespuesta.text = "";
        StartCoroutine(ActivarInput());
    }
    public void cerrarPanel()
    {
        CerrarPanel(true);
    }

    void CerrarPanel(bool reanudarMundo)
    {
        panelAbierto = false;
        panelPregunta.SetActive(false);
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        if (reanudarMundo)
            ReactivarJugador();
    }

    IEnumerator ActivarInput()
    {
        yield return new WaitForSeconds(0.2f);
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForSeconds(0.1f);
        EventSystem.current.SetSelectedGameObject(inputRespuesta.gameObject);
        inputRespuesta.ActivateInputField();
        inputRespuesta.Select();
    }

    void GenerarPregunta()
    {
        int operacion;

        if (tipoOperacion == TipoOperacion.Suma)
            operacion = 0;
        else if (tipoOperacion == TipoOperacion.Resta)
            operacion = 1;
        else if (tipoOperacion == TipoOperacion.Multiplicacion)
            operacion = 2;
        else if (tipoOperacion == TipoOperacion.Division)
            operacion = 3;
        else
            operacion = UnityEngine.Random.Range(0, 4);

        switch (operacion)
        {
            case 0: // Suma
                int a0 = UnityEngine.Random.Range(1, 20);
                int b0 = UnityEngine.Random.Range(1, 20);
                textoPregunta.text = $"¿Cuánto es {a0} + {b0}?";
                respuestaCorrecta = a0 + b0;
                break;

            case 1: // Resta
                int a1 = UnityEngine.Random.Range(1, 20);
                int b1 = UnityEngine.Random.Range(1, 20);
                if (a1 < b1) { int temp = a1; a1 = b1; b1 = temp; }
                textoPregunta.text = $"¿Cuánto es {a1} - {b1}?";
                respuestaCorrecta = a1 - b1;
                break;

            case 2: // Multiplicacion
                int a2 = UnityEngine.Random.Range(1, 12);
                int b2 = UnityEngine.Random.Range(1, 12);
                textoPregunta.text = $"¿Cuánto es {a2} x {b2}?";
                respuestaCorrecta = a2 * b2;
                break;

            case 3: // Division
                int b3 = UnityEngine.Random.Range(1, 10);
                int resultado = UnityEngine.Random.Range(1, 10);
                int a3 = b3 * resultado; // asegurar division exacta
                textoPregunta.text = $"¿Cuánto es {a3} ÷ {b3}?";
                respuestaCorrecta = resultado;
                break;
        }

        textoIntentos.text = $"Intentos: {intentosRestantes}/{intentosMaximos}";
        textoFeedback.text = "";
    }

    void ReactivarJugador()
    {
        PausarJugador(false);

        PausarEnemigos(false);
    }

    public void VerificarRespuesta()
    {
        if (int.TryParse(inputRespuesta.text, out int respuesta))
        {
            if (respuesta == respuestaCorrecta)
            {
                textoFeedback.text = "¡Correcto! Avanzando...";
                respuestaValida = true;
                CerrarPanel(true);
                // Notificar que la respuesta fue correcta
                OnRespuestaResuelta?.Invoke(true);
            }
            else
            {
                intentosRestantes--;
                textoIntentos.text = $"Intentos: {intentosRestantes}/{intentosMaximos}";

                if (intentosRestantes <= 0)
                {
                    textoFeedback.text = "¡Sin intentos! Reiniciando...";
                    bool jugadorMurio = PerderVida();
                    CerrarPanel(!jugadorMurio);
                    // Notificar que falló la pregunta
                    OnRespuestaResuelta?.Invoke(false);
                }
                else
                {
                    textoFeedback.text = $"Incorrecto. Te quedan {intentosRestantes} intentos.";
                    inputRespuesta.text = "";
                    StartCoroutine(ActivarInput());
                }
            }
        }
        else
        {
            textoFeedback.text = "Escribe un número válido.";
        }
    }

    bool PerderVida()
    {
        PlayerController pc = ObtenerJugador();
        if (pc != null)
        {
            pc.vida--;

            if (pc.vida <= 0)
            {
                pc.muerto = true;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver();
                }

                return true;
            }
        }

        return false;
    }
}
