using UnityEngine;

public class EnemySoundController : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip sonidoAtacar; 
    public AudioClip sonidoMuerte; 
    public AudioClip sonidoMov1; 
    public AudioClip sonidoMov2;


    public void playAtacar()
    {
        audioSource.PlayOneShot(sonidoAtacar);
    }

    public void playMuerte()
    {
        audioSource.PlayOneShot(sonidoMuerte);
    }

    public void playMov1()
    {
        audioSource.PlayOneShot(sonidoMov1);
    }

    public void playMov2()
    {
        audioSource.PlayOneShot(sonidoMov2);
    }
}
