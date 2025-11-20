using UnityEngine;

public class Bala : MonoBehaviour
{
    [Header("Configuración de Bala")]
    public float velocidad = 10f;
    public float lifeTime = 3f;
    public float daño = 1f; // daño de esta bala

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Movimiento constante hacia arriba 
        transform.Translate(Vector3.up * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si golpea a un enemigo, le hace daño
        var vidaEnemigo = other.GetComponent<VIDAEnemigo>();
        if (vidaEnemigo != null)
        {
            vidaEnemigo.TakeHit(daño);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
