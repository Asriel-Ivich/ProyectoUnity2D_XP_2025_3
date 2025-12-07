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

    [Header("Daño")]
    public float tiempoDaño = 0.2f; //  define cuánto dura la animación 

    private Rigidbody2D rb;
    private Animator anim;

    private bool canDoubleJump;
    private bool isGrounded;
    private bool isDead;

    private bool isHurt;      // indica si está en animación de daño
    private float dañoTimer;  //  cuánto tiempo dura el daño
    public PlayerAudioController audioController; // Llama al udiocontroller

    private void Awake()
    {
      
        rb = GetComponent<Rigidbody2D>();

        audioController = GetComponent<PlayerAudioController>();//Lo busca

        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        
        if (isDead) return;

        
        Mover();

       
        Saltar();

        
        ActualizarAnimaciones();

        
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Test: forzando DAMAGE con tecla H");
            ReproducirAnimacionDaño();
        }
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
        // revisa si toca el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                if (audioController != null)
                {
                    audioController.ReproducirSalto();  // esto llama al sonido de salto
                }
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
        //  aplica velocidad vertical de salto
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
    }

    private void ActualizarAnimaciones()
    {
        if (anim == null) return;

  
        if (isHurt)
        {
            dañoTimer -= Time.deltaTime;

           
            if (dañoTimer <= 0f)
            {
                isHurt = false;
            }

            
            return;
        }

     
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

    public void ReproducirAnimacionDaño()
    {
        
        if (anim == null)
        {
            Debug.LogWarning("No se encontró Animator en CharacterControler");
            return;
        }

        // activala animacion
        isHurt = true;

        // reinicia  la animación 
        dañoTimer = tiempoDaño;

        //llama a la animación 
        Debug.Log("Reproduciendo animación DAMAGE");
        anim.Play("DAMAGE");
    }

    public void MarcarComoMuerto()
    {
        
        isDead = true;
    }

    private void OnDrawGizmosSelected()
    {
        // verificación de suelo
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}


