using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private PlayerController playerController;
    private float vidaMaxima;

    void Start()
    {
        BuscarJugador();
    }

    void Update()
    {
        if (rellenoBarraVida == null)
        {
            return;

        }

        if (playerController == null)
        {
            BuscarJugador();
        }

        if (playerController != null && vidaMaxima > 0)
        {
            rellenoBarraVida.fillAmount = (float)playerController.vida / vidaMaxima;
        }
    }

    private void BuscarJugador()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            vidaMaxima = playerController.vida;
        }
    }
}
