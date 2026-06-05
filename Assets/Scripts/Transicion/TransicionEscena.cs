using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicionEscena : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AnimationClip animacionFinal;
    private bool cargando = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void CargarEscena(string nombreEscena)
    {
        if (cargando) return;
        cargando = true;

        ActivarEnLaJerarquia();

        if (animator == null) animator = GetComponent<Animator>();
        if (animator != null) animator.SetTrigger("Iniciar");

        MonoBehaviour runner = (GameManager.Instance != null) ? (MonoBehaviour)GameManager.Instance : this;
        runner.StartCoroutine(EsperarYCambiar(nombreEscena));
    }

    private void ActivarEnLaJerarquia()
    {
        if (gameObject.activeInHierarchy) return;

        Transform t = transform.parent;
        while (t != null)
        {
            if (!t.gameObject.activeSelf) t.gameObject.SetActive(true);
            t = t.parent;
        }
        gameObject.SetActive(true);
    }

    private IEnumerator EsperarYCambiar(string nombreEscena)
    {
        float waitTime = animacionFinal != null ? animacionFinal.length : 0f;
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(nombreEscena);
    }
}
