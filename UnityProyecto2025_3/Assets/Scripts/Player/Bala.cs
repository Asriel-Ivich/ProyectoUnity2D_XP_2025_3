using UnityEngine;

public class Bala : MonoBehaviour
{
    [Header("Configuración de Bala")]
    public float velocidad = 10f;
    public float lifeTime = 3f;
    //public float hit = 1; //Daño que hace la bala

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

    //Al Chocar con el enemigo le hace daño
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    var enemigo = collision.collider.GetComponent<VIDAEnemigo>();
    //    if (enemigo)
    //    {
    //        enemigo.TakeHit(hit); //Hace el daño que le pongamos
    //    }
    //    Destroy(gameObject);
   // }
}
