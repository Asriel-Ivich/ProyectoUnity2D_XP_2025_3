using UnityEngine;

public class PlayerVida : MonoBehaviour
{
    public int vidaMaxima = 20;
    public int vidaActual;

    private CharacterControler characterControler;//Animacion
    public PlayerAudioController audioController;// audio

    private void Awake()
    {

        audioController = GetComponent<PlayerAudioController>();  //  busca el script de audio
        
        characterControler = GetComponent<CharacterControler>(); //  obtiene la referencia al controlador del personaje

        if (characterControler == null)
        {
            Debug.LogWarning("PlayerVida no encontró CharacterControler en el mismo objeto");
        }
    }

    private void Start()
    {

        vidaActual = vidaMaxima;
    }

    public void RecibirDanio(int daño)
    {
        Debug.Log($"Player recibe {daño} de daño. Vida antes: {vidaActual}");

        vidaActual -= daño;

        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        Debug.Log($"Vida después: {vidaActual}");

        //  llama a la animación 
        if (vidaActual > 0 && characterControler != null)
        {
            
            characterControler.ReproducirAnimacionDaño();
        }

        // esto llama al sonido de daño
        if (audioController != null)
        {
            audioController.ReproducirDaño();  
        }
        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    public void RecuperarVida(int cantidad)
    {
        int vidaAnterior = vidaActual;

        vidaActual += cantidad;

        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        int vidaRealRecuperada = vidaActual - vidaAnterior;

        Debug.Log($"Player recuperó {vidaRealRecuperada} de vida. Vida actual: {vidaActual}/{vidaMaxima}");
    }

    private void Morir()
    {

        Debug.Log("Player murió.");

        //  marca al personaje como muerto en el controlador
        if (characterControler != null)
        {
            characterControler.MarcarComoMuerto();
        }

        
        Pausa pauseManager = FindObjectOfType<Pausa>();
        if (pauseManager != null)
        {
           //llama a la pantalla de Game Over
            pauseManager.MostrarGameOver();
        }
        else
        {
       
            Debug.LogError("No se encontró PauseManager en la escena");
        }

    
        gameObject.SetActive(false);
    }
}
