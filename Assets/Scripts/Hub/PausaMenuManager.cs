using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausaMenuManager : MonoBehaviour
{
    public GameObject panelPausa;
    private bool pausado = false;
    private bool inicializado = false;

    void Start()
    {
        if (panelPausa == null)
            panelPausa = GameObject.Find("PanelPausa");

        if (!inicializado)
        {
            Button[] botones = GetComponentsInChildren<Button>(true);
            foreach (Button btn in botones)
            {
                string nombre = btn.name.Trim();
                switch (nombre)
                {
                    case "BtnPausa":
                        btn.onClick.AddListener(TogglePausa);
                        break;
                    case "BtnContinuar":
                        btn.onClick.AddListener(Continuar);
                        break;
                    case "BtnReiniciar":
                        btn.onClick.AddListener(ReiniciarNivel);
                        break;
                    case "BtnNiveles":
                        btn.onClick.AddListener(IrMenuNiveles);
                        break;
                    case "BtnInicio":
                        btn.onClick.AddListener(IrMenuInicial);
                        break;
                }
            }
            inicializado = true;
        }

        if (panelPausa != null)
            panelPausa.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePausa();
    }

    public void TogglePausa()
    {
        if (panelPausa == null) return;
        pausado = !pausado;
        panelPausa.SetActive(pausado);
        Time.timeScale = pausado ? 0f : 1f;
    }

    public void Continuar()
    {
        if (panelPausa == null) return;
        pausado = false;
        panelPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrMenuNiveles()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SelectLevel");
    }

    public void IrMenuInicial()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInitial");
    }
}