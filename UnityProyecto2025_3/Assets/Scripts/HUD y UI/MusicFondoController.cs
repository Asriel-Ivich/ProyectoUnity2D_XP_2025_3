using UnityEngine;

public class MusicaFondoController : MonoBehaviour
{
    public static MusicaFondoController Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
     
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        audioSource = GetComponent<AudioSource>();

        
    }

    public void ReproducirMusica()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void DetenerMusica()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

