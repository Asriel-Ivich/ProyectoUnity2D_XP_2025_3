using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    [Header("Movimiento")]
    public float velMovimeinto = 5f;

    [Header("Salto")]
    public float salto = 10f;
    public float dobleSaltoF = 8f;

    [Header("Verificación Suelo")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private Animator anim;

    private bool canDoubleJump;
    private bool isGrounded;
    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Mover();
        Saltar();
        ActualizarAnimaciones();
    }

    private void Mover()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(inputX * velMovimeinto, rb.linearVelocity.y);

        
        if (inputX > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (inputX < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void Saltar()
    {
        // Detecta suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                AplicarSalto(salto);
            }
            else if (canDoubleJump)
            {
                AplicarSalto(dobleSaltoF);
                canDoubleJump = false;
            }
        }
    }

    private void AplicarSalto(float force)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
    }

    // se llaman a las animaciones
    private void ActualizarAnimaciones()
    {
        if (anim == null) return;

       

        float velX = Mathf.Abs(rb.linearVelocity.x);
        float velY = rb.linearVelocity.y;

        if (!isGrounded && velY > 0.1f)
        {
            anim.Play("SALTO");
        }
        else if (!isGrounded && velY < -0.1f)
        {
            anim.Play("CAIDA");
        }
        else if (velX > 0.1f)
        {
            anim.Play("WALK");
        }
        else
        {
            anim.Play("IDLE");
        }
    }

    

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}

