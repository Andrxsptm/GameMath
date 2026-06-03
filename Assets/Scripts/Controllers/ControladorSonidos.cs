using UnityEngine;

public class ControladorSonidos : MonoBehaviour
{
    public static ControladorSonidos instance;
    private AudioSource audioSource;
    private void Awake()
    {
       
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }
    public void ejecutarSonido(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
