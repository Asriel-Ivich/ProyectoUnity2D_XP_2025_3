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

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoDisparo;

    private float tiempoSiguienteDisparo;

    private void Awake()
    {
        // Busca un AudioSource en el mismo objeto si no se asigna desde el Inspector
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

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
        // Verifica referencias necesarias para disparar
        if (proyectilPrefab == null || puntoDisparo == null)
        {
            Debug.LogWarning($"{name}: faltan referencias al prefab o punto de disparo.");
            return;
        }

        // Instancia del proyectil en el punto definido
        var proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);

        // Asigna velocidad hacia abajo 
        if (proyectil.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.down * fuerzaDisparo;
        }

        // Asigna daño dinámicamente si el prefab tiene el componente DisparoEnemigo
        if (proyectil.TryGetComponent<DisparoEnemigo>(out var script))
        {
            script.Daño = dañoDelProyectil;
        }

        // Llama al audio del disparo
        ReproducirSonidoDisparo();
    }

    private void ReproducirSonidoDisparo()
    {
        // Verifica que existan referencias de audio
        if (audioSource != null && sonidoDisparo != null)
        {
            // Reproduce el sonido del disparo
            audioSource.PlayOneShot(sonidoDisparo);
        }
    }
}

