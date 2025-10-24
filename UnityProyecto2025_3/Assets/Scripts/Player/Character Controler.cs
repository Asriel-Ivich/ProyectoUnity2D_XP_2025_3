using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    [Header("Movimiento")]
    public float velMovimeinto = 5f;

    [Header("Salto")]
    public float salto = 10f;
    public float dobleSaltoF = 8f;

    [Header("Verificaci�n Suelo")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool dobleSalto;
    private float movHorizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Verificar si la ranita esta en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // Input del movimiento
        movHorizontal = Input.GetAxis("Horizontal");

        // Salto
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump(salto);
                dobleSalto = true;
            }
            else if (dobleSalto)
            {
                Jump(dobleSaltoF);
                dobleSalto = false;
            }
        }
    }

    void FixedUpdate()
    {
        // Movimiento horizontal de la ranita
        rb.linearVelocity = new Vector2(movHorizontal * velMovimeinto, rb.linearVelocity.y);
    }

    void Jump(float force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
    }

    // Area del detector del suelo
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
