using UnityEngine;

public class PlayerVida : MonoBehaviour
{
    public int vidaMaxima = 20;
    public int vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que colisiona tiene la layer "Disparo Enemigo"
        if (other.gameObject.layer == LayerMask.NameToLayer("Disparo Enemigo"))
        {
            DisparoEnemigo disparo = other.GetComponent<DisparoEnemigo>();
            if (disparo != null)
            {
                RecibirDanio(Mathf.RoundToInt(disparo.Daño));
            }
            Destroy(other.gameObject); // Destruir el disparo
        }
    }*/

    public void RecibirDanio(int daño)
    {
        Debug.Log($"Player recibe {daño} de daño. Vida antes: {vidaActual}");

        vidaActual -= daño;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        Debug.Log($"Vida después: {vidaActual}");

        if (vidaActual <= 0)
        {
            Debug.Log("Player murió.");
            Destroy(gameObject);
        }
    }

}
