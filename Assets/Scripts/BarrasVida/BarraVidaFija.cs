using UnityEngine;
using UnityEngine.UI;

public class BarraVidaFija : MonoBehaviour
{
    public Image rellenoBarraVida;
    private EnemyController EnemyController;
    private float vidaMaxima;

    void Start()
    {
        BuscarEnemigo();
    }

    void Update()
    {
        if (rellenoBarraVida == null)
        {
            return;
        }

        if (EnemyController == null)
        {
            BuscarEnemigo();
        }

        if (EnemyController != null && vidaMaxima > 0)
        {
            rellenoBarraVida.fillAmount = (float)EnemyController.vida / vidaMaxima;
        }
    }

    private void BuscarEnemigo()
    {
        EnemyController = FindFirstObjectByType<EnemyController>();
        if (EnemyController != null)
        {
            vidaMaxima = EnemyController.vida;
        }
    }
}

