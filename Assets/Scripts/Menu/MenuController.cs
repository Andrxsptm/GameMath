using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public void Play()
    {
        PartidaGuardada.Limpiar();
        SceneManager.LoadScene("SelectLevel");
        Debug.Log("presionado");
    }

    void Continue()
    {
        string escena = PartidaGuardada.ObtenerEscena();
        if (!string.IsNullOrEmpty(escena))
        {
            PartidaGuardada.MarcarRestauracion();
            SceneManager.LoadScene(escena);
        }
    }

    void Start()
    {
        GameObject btnContinue = GameObject.Find("btn_Continue");
        if (btnContinue != null)
        {
            btnContinue.SetActive(PartidaGuardada.Existe());
            Button btn = btnContinue.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(Continue);
        }

        GameObject btnPlay = GameObject.Find("btn_Play");
        if (btnPlay != null)
        {
            Button btn = btnPlay.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(PartidaGuardada.Limpiar);
        }
    }
}
