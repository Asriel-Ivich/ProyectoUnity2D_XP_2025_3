/*using UnityEngine;

public class PlayerVida : MonoBehaviour
{
    public int vidaMaxima = 20;
    public int vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que colisiona tiene la layer "Disparo Enemigo"
        if (other.gameObject.layer == LayerMask.NameToLayer("Disparo Enemigo"))
        {
            DisparoEnemigo disparo = other.GetComponent<DisparoEnemigo>();
            if (disparo != null)
            {
                RecibirDanio(disparo.danio);
            }
            Destroy(other.gameObject); // Destruir el disparo
        }
    }

    public void RecibirDanio(int daño)
    {
        vidaActual -= daño;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        if (vidaActual <= 0)
        {
            Destroy(gameObject); // La Morision del player
        }
    }
}
*/