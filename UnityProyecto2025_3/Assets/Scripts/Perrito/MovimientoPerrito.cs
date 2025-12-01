using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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

    // Componentes
    private Rigidbody2D rb;
    private PerritoSpawner spawner;

    // Control de movimiento
    private int direccion = 1;
    private float temporizadorVida;
    private bool activo = true;
    private Collider2D plataformaActual;
    private Vector2 limitesPlataforma;

    // Prevención de bug
    private float tiempoUltimoCambio = 0f;
    private float tiempoMinimoEntreCambios = 0.5f;
    private int intentosFallidos = 0;
    private const int MAX_INTENTOS_FALLIDOS = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        temporizadorVida = tiempoDeVida;

        // Configurar rigidbody
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        // Detectar plataforma inicial
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

        // Actualizar límites de la plataforma
        ActualizarLimitesPlataforma();

        // Verificar si necesita cambiar dirección
        if (Time.time - tiempoUltimoCambio >= tiempoMinimoEntreCambios)
        {
            VerificarBordePlataforma();
        }

        // Mover al perro
        MoverPerro();
    }

    void DetectarPlataformaActual()
    {
        // Usar un circle cast para detectar la plataforma debajo
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

                // Debug visual
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

        // Debug visual
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

        // Calcular posición actual relativa a los límites
        float posX = transform.position.x;

        // Si estamos muy cerca del borde izquierdo
        if (posX <= limitesPlataforma.x + distanciaMinimaBorde)
        {
            // Si vamos hacia la izquierda, cambiar a derecha
            if (direccion == -1)
            {
                CambiarDireccion();
            }
        }
        // Si estamos muy cerca del borde derecho
        else if (posX >= limitesPlataforma.y - distanciaMinimaBorde)
        {
            // Si vamos hacia la derecha, cambiar a izquierda
            if (direccion == 1)
            {
                CambiarDireccion();
            }
        }
        else
        {
            // Si estamos en el centro, asegurarnos de que estamos sobre la plataforma
            VerificarSiSigueEnPlataforma();
        }
    }

    void VerificarSiSigueEnPlataforma()
    {
        // Verificar si todavía estamos sobre la plataforma
        Vector2 origen = transform.position;
        float distancia = 0.3f;

        RaycastHit2D hit = Physics2D.Raycast(origen, Vector2.down, distancia, layerPisos);

        if (hit.collider == null || hit.collider != plataformaActual)
        {
            // Buscar nueva plataforma
            DetectarPlataformaActual();
        }
    }

    void MoverPerro()
    {
        if (plataformaActual == null) return;

        // Calcular nueva posición
        float nuevaVelocidadX = direccion * velocidadMovimiento;

        // Aplicar movimiento
        Vector2 velocidadActual = rb.linearVelocity;
        rb.linearVelocity = new Vector2(nuevaVelocidadX, velocidadActual.y);

        // Sincronizar con plataforma móvil
        if (plataformaActual.TryGetComponent<PlataformaMOVIL>(out PlataformaMOVIL plataformaMovil))
        {
            // El movimiento en Y ya lo maneja el parent, solo nos aseguramos de seguirla
            // En plataformas móviles, ajustamos manualmente la posición
            Vector2 velocidadPlataforma = plataformaMovil.GetComponent<Rigidbody2D>()?.linearVelocity ?? Vector2.zero;
            rb.linearVelocity += new Vector2(0, velocidadPlataforma.y);
        }
    }

    void CambiarDireccion()
    {
        tiempoUltimoCambio = Time.time;
        direccion *= -1;

        // Voltear sprite
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
                DestruirItem();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si colisiona con algo que no es player, podría ser una pared
        if (collision.gameObject.layer == LayerMask.NameToLayer("Piso") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Plataforma"))
        {
            // Si hay colisión frontal, cambiar dirección
            ContactPoint2D contacto = collision.GetContact(0);
            if (Mathf.Abs(contacto.normal.x) > 0.5f)
            {
                CambiarDireccion();
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

        activo = false;

        if (spawner != null)
        {
            spawner.PerroDestruido();
        }

        Destroy(gameObject);
    }

    // Debug visual
    void OnDrawGizmos()
    {
        if (!Application.isPlaying || !activo) return;

        // Dibujar rayos de detección
        Gizmos.color = Color.green;
        Vector2 origen = transform.position;
        Gizmos.DrawWireSphere(origen, 0.2f);
        Gizmos.DrawLine(origen, origen + Vector2.down * 0.5f);

        // Dibujar límites de plataforma
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