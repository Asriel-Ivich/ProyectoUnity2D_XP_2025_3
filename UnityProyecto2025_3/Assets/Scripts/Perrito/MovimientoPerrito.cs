using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoPerrito : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    [SerializeField] private float velocidadHorizontal = 2f;
    [SerializeField] private float distanciaPatrulla = 3f;

    [Header("Configuración de Vida")]
    [SerializeField] private float tiempoDeVida = 7f;
    [SerializeField] private int vidaParaRecuperar = 5;

    private Rigidbody2D rb;
    private Vector2 puntoInicio;
    private int direccion = 1;
    private PerritoSpawner spawner;
    private float temporizadorVida;
    private bool activo = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        puntoInicio = rb.position;
        temporizadorVida = tiempoDeVida;

        ConfigurarRigidbody();
    }

    void ConfigurarRigidbody()
    {
        rb.gravityScale = 0f; 
        rb.freezeRotation = true; 
    }

    private void FixedUpdate()
    {
        if (!activo) return;

        rb.linearVelocity = new Vector2(direccion * velocidadHorizontal, 0);

        float distanciaRecorrida = rb.position.x - puntoInicio.x;

        if (direccion == 1 && distanciaRecorrida >= distanciaPatrulla)
            CambiarDireccion();
        else if (direccion == -1 && distanciaRecorrida <= -distanciaPatrulla)
            CambiarDireccion();

        temporizadorVida -= Time.fixedDeltaTime;
        if (temporizadorVida <= 0f)
        {
            DestruirItem();
        }
    }

    private void CambiarDireccion()
    {
        direccion *= -1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && activo)
        {
            PlayerVida playerVida = other.GetComponent<PlayerVida>();
            if (playerVida != null)
            {
                playerVida.RecuperarVida(vidaParaRecuperar);
                Debug.Log($"Player recuperó {vidaParaRecuperar} de vida");

                DestruirItem();
            }
        }
    }

    public void RecuperarVida(int cantidad)
    {
        Debug.Log($"Item ofrece {cantidad} de vida");
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
            spawner.ItemDestruido();
        }

        Debug.Log("Item destruido/recogido");

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying && activo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine((Vector2)transform.position - Vector2.right * distanciaPatrulla,
                            (Vector2)transform.position + Vector2.right * distanciaPatrulla);
        }
    }
}
