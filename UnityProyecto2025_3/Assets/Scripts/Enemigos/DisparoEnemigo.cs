using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DisparoEnemigo : MonoBehaviour
{
    [Header("Configuración del proyectil")]
    [SerializeField, Min(0f)] private float daño = 5f;
    [SerializeField, Min(0.1f)] private float tiempoVida = 3f;

    public float Daño
    {
        get => daño;
        set => daño = Mathf.Max(0f, value);  
    }


    private void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Usamos la LAYER "Jugador"
        if (other.gameObject.layer == LayerMask.NameToLayer("Jugador"))
        {
            PlayerVida vida = other.GetComponent<PlayerVida>();
            if (vida != null)
            {
                vida.RecibirDanio(Mathf.RoundToInt(daño));
            }

            Destroy(gameObject); // La bala se destruye al impactar
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}


