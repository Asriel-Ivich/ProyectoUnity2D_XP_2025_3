using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [Header("Fuente de audio")]
    public AudioSource audioSource;  

    [Header("Sonidos")]
    public AudioClip sonidoDaño;     
    public AudioClip sonidoSalto;    
    public AudioClip sonidoDisparo;  

    private void Awake()
    {
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void ReproducirDaño()
    {
        //  sonido de daño
        ReproducirClip(sonidoDaño);
    }

    public void ReproducirSalto()
    {
        //  sonido de salto
        ReproducirClip(sonidoSalto);
    }

    public void ReproducirDisparo()
    {
        //  sonido de disparo
        ReproducirClip(sonidoDisparo);
    }

    private void ReproducirClip(AudioClip clip)
    {
        
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
