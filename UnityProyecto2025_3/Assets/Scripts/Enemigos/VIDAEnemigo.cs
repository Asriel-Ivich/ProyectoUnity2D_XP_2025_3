using UnityEngine;

public class VIDAEnemigo : MonoBehaviour 
{
    public float PuntosVida;
    public float VidaMaxima = 2; //Vida maxima
    public float hit = 1;

    void Start()
    {
        PuntosVida = VidaMaxima;
    }

    public void TakeHit (float golpe)
    {
        PuntosVida -= golpe;
        if (PuntosVida <= 0)
        {
            Destroy(gameObject);
        }
    }

    
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
    
}
