using UnityEngine;

public class VIDAEnemigo : MonoBehaviour
{
    [Header("Vida")]
    public float VidaMaxima = 2f;
    public float PuntosVida;

    private Animator anim;
    private bool muerto = false;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>(); 
    }

    void Start()
    {
        PuntosVida = VidaMaxima;
    }

    public void TakeHit(float golpe)
    {
        if (muerto) return;

        PuntosVida -= golpe;

        // Animación de daño
        if (anim != null)
        {
            anim.Play("Daño");
        }

        if (PuntosVida <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        muerto = true;

        // Opcional: apagar movimiento / disparo si los usas
        var patrol = GetComponent<EnemyPatrolVertical>();
        if (patrol != null) patrol.enabled = false;

        var shooter = GetComponent<EnemyShooter>();
        if (shooter != null) shooter.enabled = false;

        if (anim != null)
        {
            anim.Play("Muerte");
        }

        Destroy(gameObject, 0.5f); // se destruye tras la animación
    }

    /*
    //Al Chocar con el enemigo le hace daño
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemigo = collision.collider.GetComponent<VIDAEnemigo>();
        if (enemigo)
        {
            enemigo.TakeHit(hit); 
        }
       Destroy(gameObject);
     }
    */
}
