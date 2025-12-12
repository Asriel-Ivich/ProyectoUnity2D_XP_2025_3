using UnityEngine;

public class ZonaDerrota : MonoBehaviour
{
    [Header("Detección de Enemigos")]
    [Tooltip("Tag que usarán los enemigos (por ejemplo: Enemy).")]
    [SerializeField] private string tagEnemigo = "Enemy";

    private Pausa pauseManager;
    private bool derrotaActivada = false;

    private void Awake()
    {
        /* Busca el script Pausa en la escena
        pauseManager = FindObjectOfType<Pausa>();*/
        pauseManager = Object.FindFirstObjectByType<Pausa>();

        if (pauseManager == null)
        {
            Debug.LogError("ZonaDerrota: No se encontró un objeto con el script Pausa en la escena.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (derrotaActivada) return; // Evita que se hagan varias calls

        // 
        if (other.CompareTag(tagEnemigo))
        {
            derrotaActivada = true;

            Debug.Log("¡Derrota! Un enemigo llegó al fondo de la escena.");

            if (pauseManager != null)
            {
                pauseManager.MostrarGameOver();
            }

            // destruiye al enemigo que llego
            Destroy(other.gameObject);
        }
    }
}
