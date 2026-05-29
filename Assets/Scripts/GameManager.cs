using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;
    public GameObject panelWin;
    public TextMeshProUGUI gameOverText;
    public Button reiniciarButton;
    public Button menuButton;
    private bool gameOverActivo = false;

    [Header("Monedas")]
    public int monedas = 0;
    public Image monedaIcon;
    public TextMeshProUGUI MonedasText;

    [Header("Puntos")]
    public int puntos = 0;
    public TextMeshProUGUI PuntosText;

    private PlayerController jugador;
    private bool interfazVisible = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (panelWin != null)
        {
            panelWin.SetActive(false);
        }
        if (reiniciarButton != null)
        {
            reiniciarButton.onClick.AddListener(ReiniciarJuego);
        }
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(VolverAlMenu);
        }
        
        cargarMonedayPuntos();
        ActualizarMonedasyPuntos();
        BuscarJugador();
        ActualizarEstadoUIJugador();
    }

    // Update is called once per frame
    void Update()

    {
        BuscarJugador();
        ActualizarEstadoUIJugador();
        
        if (gameOverActivo)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReiniciarJuego();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                VolverAlMenu();
            }
        }
    }
    public void SumarMoneda(int cantidad)
    {
        monedas+=cantidad;
        ActualizarMonedasyPuntos();
        guardarMonedayPuntos();
    }
    void ActualizarMonedasyPuntos()
    {
        if (MonedasText != null)
        {
            MonedasText.text = "x" + monedas.ToString();
        }
        if (PuntosText != null)
        {
            PuntosText.text = "Pts: " + puntos.ToString();
        }
    }

    void BuscarJugador()
    {
        if (jugador == null)
        {
            jugador = FindObjectOfType<PlayerController>();
        }
    }

    void ActualizarEstadoUIJugador()
    {
        bool jugadorMuerto = jugador != null && jugador.muerto;
        bool debeMostrarse = !jugadorMuerto;

        if (interfazVisible == debeMostrarse)
            return;

        interfazVisible = debeMostrarse;

        if (MonedasText != null)
            MonedasText.gameObject.SetActive(debeMostrarse);

        if (PuntosText != null)
            PuntosText.gameObject.SetActive(debeMostrarse);

        if (monedaIcon != null)
            monedaIcon.gameObject.SetActive(debeMostrarse);
    }
    void guardarMonedayPuntos()
    {
        PlayerPrefs.SetInt("Monedas", monedas);
        PlayerPrefs.SetInt("Puntos", puntos);
        PlayerPrefs.Save();
    }
    void cargarMonedayPuntos()
    {
            monedas = PlayerPrefs.GetInt("Monedas", 0);
            puntos = PlayerPrefs.GetInt("Puntos", 0);
            ActualizarMonedasyPuntos();
        
    }
    public void GameOver()
    {
       if (gameOverActivo) return;

        gameOverActivo = true;
        interfazVisible = false;

        if (MonedasText != null)
            MonedasText.gameObject.SetActive(false);

        if (PuntosText != null)
            PuntosText.gameObject.SetActive(false);

        if (monedaIcon != null)
            monedaIcon.gameObject.SetActive(false);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        if (gameOverText != null)
        {
            gameOverText.text = "Perdiste!";
        }
    }

    public void AbrirPanelWin()
    {
        if (panelWin != null)
        {
            panelWin.SetActive(true);
        }
    }

    private void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SelectLevel");
    }

}


