using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))] // Añadimos el requerimiento de AudioSource
public class MovimientoPerrito : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 2f;
    [SerializeField] private float distanciaMinimaBorde = 0.3f;

    [Header("Configuración")]
    [SerializeField] private float tiempoDeVida = 10f;
    [SerializeField] private int vidaParaRecuperar = 5;
    [SerializeField] private LayerMask layerPisos;

    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Sonido")]
    [SerializeField] private AudioClip sonidoRecoleccion; // Sonido cuando el jugador toca al perrito
    [SerializeField] private float volumenSonido = 1f; // Volumen del sonido (0 a 1)
    [SerializeField] private bool destruirDespuesDelSonido = true; // Si se debe esperar a que termine el sonido

    private Rigidbody2D rb;
    private PerritoSpawner spawner;
    private AudioSource audioSource; // Referencia al AudioSource

    private int direccion = 1;
    private float temporizadorVida;
    private bool activo = true;
    private Collider2D plataformaActual;
    private Vector2 limitesPlataforma;

    private float tiempoUltimoCambio = 0f;
    private float tiempoMinimoEntreCambios = 0.5f;
    private int intentosFallidos = 0;
    private const int MAX_INTENTOS_FALLIDOS = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Obtener o crear AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        temporizadorVida = tiempoDeVida;

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        DetectarPlataformaActual();
    }

    void Update()
    {
        if (!activo) return;

        temporizadorVida -= Time.deltaTime;
        if (temporizadorVida <= 0f)
        {
            DestruirItem();
            return;
        }
    }

    void FixedUpdate()
    {
        if (!activo || plataformaActual == null) return;

        ActualizarLimitesPlataforma();

        if (Time.time - tiempoUltimoCambio >= tiempoMinimoEntreCambios)
        {
            VerificarBordePlataforma();
        }

        MoverPerro();
    }

    void DetectarPlataformaActual()
    {
        Vector2 origen = transform.position;
        float radio = 0.2f;
        float distancia = 0.5f;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(origen, radio, Vector2.down, distancia, layerPisos);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                plataformaActual = hit.collider;
                ActualizarLimitesPlataforma();
                intentosFallidos = 0;

                Debug.DrawRay(hit.point, Vector2.up * 0.2f, Color.green, 1f);
                break;
            }
        }

        if (plataformaActual == null)
        {
            intentosFallidos++;
            if (intentosFallidos >= MAX_INTENTOS_FALLIDOS)
            {
                Debug.LogWarning("Perro no encontró plataforma, destruyendo...");
                DestruirItem();
            }
        }
    }

    void ActualizarLimitesPlataforma()
    {
        if (plataformaActual == null) return;

        Bounds bounds = plataformaActual.bounds;
        limitesPlataforma = new Vector2(bounds.min.x, bounds.max.x);

        Debug.DrawLine(
            new Vector3(limitesPlataforma.x, transform.position.y - 0.1f, transform.position.z),
            new Vector3(limitesPlataforma.y, transform.position.y - 0.1f, transform.position.z),
            Color.yellow
        );
    }

    void VerificarBordePlataforma()
    {
        if (plataformaActual == null)
        {
            DetectarPlataformaActual();
            return;
        }

        float posX = transform.position.x;

        if (posX <= limitesPlataforma.x + distanciaMinimaBorde)
        {
            if (direccion == -1)
            {
                CambiarDireccion();
            }
        }
        else if (posX >= limitesPlataforma.y - distanciaMinimaBorde)
        {
            if (direccion == 1)
            {
                CambiarDireccion();
            }
        }
        else
        {
            VerificarSiSigueEnPlataforma();
        }
    }

    void VerificarSiSigueEnPlataforma()
    {
        Vector2 origen = transform.position;
        float distancia = 0.3f;

        RaycastHit2D hit = Physics2D.Raycast(origen, Vector2.down, distancia, layerPisos);

        if (hit.collider == null || hit.collider != plataformaActual)
        {
            DetectarPlataformaActual();
        }
    }

    void MoverPerro()
    {
        if (plataformaActual == null) return;

        float nuevaVelocidadX = direccion * velocidadMovimiento;

        Vector2 velocidadActual = rb.linearVelocity;
        rb.linearVelocity = new Vector2(nuevaVelocidadX, velocidadActual.y);

        if (plataformaActual.TryGetComponent<PlataformaMOVIL>(out PlataformaMOVIL plataformaMovil))
        {
            Vector2 velocidadPlataforma = plataformaMovil.GetComponent<Rigidbody2D>()?.linearVelocity ?? Vector2.zero;
            rb.linearVelocity += new Vector2(0, velocidadPlataforma.y);
        }
    }

    void CambiarDireccion()
    {
        tiempoUltimoCambio = Time.time;
        direccion *= -1;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!activo) return;

        if (other.CompareTag("Player"))
        {
            PlayerVida playerVida = other.GetComponent<PlayerVida>();
            if (playerVida != null)
            {
                playerVida.RecuperarVida(vidaParaRecuperar);

                // Reproducir sonido antes de destruir
                ReproducirSonidoYDestruir();
            }
        }
    }

    public void SetSpawner(PerritoSpawner itemSpawner)
    {
        spawner = itemSpawner;
    }

    void DestruirItem()
    {
        if (!activo) return;

        // Reproducir sonido antes de destruir (si hay sonido asignado)
        if (sonidoRecoleccion != null)
        {
            ReproducirSonidoYDestruir();
        }
        else
        {
            DestruirInmediatamente();
        }
    }

    // Método para reproducir sonido y luego destruir
    void ReproducirSonidoYDestruir()
    {
        if (!activo) return;

        activo = false;

        // Desactivar componentes visuales y físicos
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // Reproducir el sonido
        if (sonidoRecoleccion != null && audioSource != null)
        {
            audioSource.clip = sonidoRecoleccion;
            audioSource.volume = volumenSonido;
            audioSource.Play();

            if (destruirDespuesDelSonido)
            {
                // Esperar a que termine el sonido antes de destruir
                Destroy(gameObject, sonidoRecoleccion.length);
            }
            else
            {
                // Destruir inmediatamente (el sonido seguirá reproduciéndose si es un OneShot)
                DestruirInmediatamente();
            }
        }
        else
        {
            // Si no hay sonido, destruir inmediatamente
            DestruirInmediatamente();
        }

        // Notificar al spawner
        if (spawner != null)
        {
            spawner.PerroDestruido();
        }
    }

    // Método para destruir inmediatamente
    void DestruirInmediatamente()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Piso") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Plataforma"))
        {
            ContactPoint2D contacto = collision.GetContact(0);
            if (Mathf.Abs(contacto.normal.x) > 0.5f)
            {
                CambiarDireccion();
            }
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || !activo) return;

        Gizmos.color = Color.green;
        Vector2 origen = transform.position;
        Gizmos.DrawWireSphere(origen, 0.2f);
        Gizmos.DrawLine(origen, origen + Vector2.down * 0.5f);

        if (plataformaActual != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                new Vector3(limitesPlataforma.x, transform.position.y - 0.2f, transform.position.z),
                new Vector3(limitesPlataforma.y, transform.position.y - 0.2f, transform.position.z)
            );
        }
    }
}