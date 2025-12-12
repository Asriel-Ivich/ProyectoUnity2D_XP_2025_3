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
        // Verifica referencias 
        if (proyectilPrefab == null || puntoDisparo == null)
        {
            Debug.LogWarning($"{name}: faltan referencias al prefab o punto de disparo.");
            return;
        }

       
        var proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, Quaternion.identity);

        //  velocidad 
        if (proyectil.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.down * fuerzaDisparo;
        }

        //  da;o 
        if (proyectil.TryGetComponent<DisparoEnemigo>(out var script))
        {
            script.Daño = dañoDelProyectil;
        }

        // Llama audio del disparo
        ReproducirSonidoDisparo();
    }

    private void ReproducirSonidoDisparo()
    {
        
        if (audioSource != null && sonidoDisparo != null)
        {
            // Reproduce  sonido del disparo
            audioSource.PlayOneShot(sonidoDisparo);
        }
    }
}

