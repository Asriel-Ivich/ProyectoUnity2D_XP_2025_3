using UnityEngine;

[RequireComponent(typeof(Transform))]
public class EnemyShooter : MonoBehaviour
{
    [Header("Configuración del disparo")]
    [Tooltip("Prefab del proyectil que el enemigo dispara.")]
    [SerializeField] private GameObject proyectilPrefab;

    [Tooltip("Punto desde el que se generará el proyectil.")]
    [SerializeField] private Transform puntoDisparo;

    [Tooltip("Velocidad con la que se lanza el proyectil hacia abajo.")]
    [SerializeField, Min(0.1f)] private float fuerzaDisparo = 5f;

    [Tooltip("Tiempo entre cada disparo (segundos).")]
    [SerializeField, Min(0.1f)] private float tiempoEntreDisparos = 2f;

    [Header("Daño del proyectil")]
    [SerializeField, Min(0f)] private float dañoDelProyectil = 10f;

    private float tiempoSiguienteDisparo;

    private void Update()
    {
        // Control del ritmo de disparo
        if (Time.time >= tiempoSiguienteDisparo)
        {
            Disparar();
            tiempoSiguienteDisparo = Time.time + tiempoEntreDisparos;
        }
    }

    private void Disparar()
    {
        if (proyectilPrefab == null || puntoDisparo == null)
        {
            Debug.LogWarning($"{name}: faltan referencias al prefab o punto de disparo.");
            return;
        }

        // Instancia del proyectil en el punto definido
        var proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);

        // Si el proyectil tiene un Rigidbody2D, aplicamos la nueva propiedad linearVelocity
        if (proyectil.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.down * fuerzaDisparo;
        }

        // Asignar daño dinámicamente si el prefab tiene el componente Projectile
        if (proyectil.TryGetComponent<Projectile>(out var script))
        {
            script.Daño = dañoDelProyectil;
        }
    }
}


