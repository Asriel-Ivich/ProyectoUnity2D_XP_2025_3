using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrolVertical : MonoBehaviour
{
    [Header("Movimiento horizontal")]
    [SerializeField] private float velocidadHorizontal = 2f;
    [SerializeField] private float distanciaPatrulla = 3f;

    [Header("Movimiento vertical (descenso)")]
    [SerializeField] private float velocidadBajada = 2f;
    [SerializeField] private float tiempoAntesDeBajar = 3f; // Tiempo antes de bajar
    [SerializeField] private float distanciaBajada = 2f;    // Cuánto baja

    private Rigidbody2D rb;
    private Vector2 puntoInicio;
    private int direccion = 1; 
    private bool bajando = false;
    private float temporizadorBajada;
    private float alturaInicial;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        puntoInicio = rb.position;
        alturaInicial = rb.position.y;
        temporizadorBajada = tiempoAntesDeBajar;
    }

    private void FixedUpdate()
    {
        if (bajando)
        {
            // movimiento  hacia abajo
            rb.linearVelocity = new Vector2(0, -velocidadBajada);

            // Detener bajada al alcanzar la distancia configurada
            if (alturaInicial - rb.position.y >= distanciaBajada)
            {
                bajando = false;
                alturaInicial = rb.position.y; // actualiza
                temporizadorBajada = tiempoAntesDeBajar; // Reinicia contador
            }
        }
        else
        {
            // Movimiento horizontal
            rb.linearVelocity = new Vector2(direccion * velocidadHorizontal, 0);

            // Patrulla horizontal
            float distanciaRecorrida = rb.position.x - puntoInicio.x;

            if (direccion == 1 && distanciaRecorrida >= distanciaPatrulla)
                CambiarDireccion();
            else if (direccion == -1 && distanciaRecorrida <= -distanciaPatrulla)
                CambiarDireccion();

            // Contador para iniciar bajada
            temporizadorBajada -= Time.fixedDeltaTime;
            if (temporizadorBajada <= 0f)
            {
                bajando = true;
            }
        }
    }

    private void CambiarDireccion()
    {
        direccion *= -1;

        // Invertir sprite visualmente
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * direccion;
        transform.localScale = escala;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Línea horizontal de patrulla
        Gizmos.DrawLine(transform.position - Vector3.right * distanciaPatrulla,
                        transform.position + Vector3.right * distanciaPatrulla);

        Gizmos.color = Color.blue;
        // Línea vertical que representa hasta dónde bajará
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * distanciaBajada);
    }
}

