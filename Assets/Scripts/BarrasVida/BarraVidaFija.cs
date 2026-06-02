using UnityEngine;
using UnityEngine.UI;

public class BarraVidaFija : MonoBehaviour
{
    public Image rellenoBarraVida;
    private IDaniable daniable;
    private float vidaMaxima;
    private Vector2 posicionInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;
        BuscarDaniable();
    }

    void Update()
    {
        if (rellenoBarraVida == null)
        {
            return;
        }

        if (daniable == null)
        {
            BuscarDaniable();
        }

        if (daniable != null && vidaMaxima > 0)
        {
            rellenoBarraVida.fillAmount = (float)daniable.vida / vidaMaxima;
        }

        ContrarrestarEscalaPadre();
    }

    private void ContrarrestarEscalaPadre()
    {
        if (transform.parent != null)
        {
            float signoX = Mathf.Sign(transform.parent.localScale.x);

            Vector3 escala = transform.localScale;
            escala.x = signoX * Mathf.Abs(escala.x);
            transform.localScale = escala;

            Vector3 posicion = transform.localPosition;
            posicion.x = posicionInicial.x * signoX;
            transform.localPosition = posicion;
        }
    }

    private void BuscarDaniable()
    {
        daniable = GetComponentInParent<IDaniable>();
        if (daniable != null)
        {
            vidaMaxima = daniable.vida;
        }
    }
}
