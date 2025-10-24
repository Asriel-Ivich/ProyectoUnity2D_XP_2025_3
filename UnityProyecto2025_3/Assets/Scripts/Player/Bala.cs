using UnityEngine;

public class Bala : MonoBehaviour
{
    [Header("Configuración de Bala")]
    public float velocidad = 10f;
    public float lifeTime = 3f;

    private void Start()
    {
        // Destruir automáticamente después del tiempo de vida
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Movimiento constante hacia arriba
        transform.Translate(Vector3.up * velocidad * Time.deltaTime);
    }
}
