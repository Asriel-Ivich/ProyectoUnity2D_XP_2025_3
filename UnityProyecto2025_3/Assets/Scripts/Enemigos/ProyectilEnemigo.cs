using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [Header("Configuración del proyectil")]
    [SerializeField, Min(0f)] private float daño = 10f;
    [SerializeField, Min(0.1f)] private float tiempoVida = 3f;

    public float Daño
    {
        get => daño;
        set => daño = Mathf.Max(0f, value);
    }

    private void Start()
    {
        // Destruye el proyectil después del tiempo de vida configurado
        Destroy(gameObject, tiempoVida);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si golpea al jugador
        if (other.CompareTag("Player"))
        {

            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            // Destruye el proyectil si toca el suelo
            Destroy(gameObject);
        }
    }
}
