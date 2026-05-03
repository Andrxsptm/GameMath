using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicionEscena : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AnimationClip animacionFinal;
    private bool cargando = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void CargarEscena(string nombreEscena)
    {
        if (cargando) return;
        cargando = true;
        StartCoroutine(CambiarEscena(nombreEscena));
    }

    IEnumerator CambiarEscena(string nombreEscena)
    {
        if (animator != null)
            animator.SetTrigger("Iniciar");
        float waitTime = animacionFinal != null ? animacionFinal.length : 0f;
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(nombreEscena);
    }
}