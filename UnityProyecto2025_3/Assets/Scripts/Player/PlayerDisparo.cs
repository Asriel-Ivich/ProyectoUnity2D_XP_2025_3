using UnityEngine;

public class PlayerDisparo : MonoBehaviour
{
    [Header("Referencia")]
    public GameObject balaPrefab;

    [Header("Configuración de Disparo")]
    public float coldown = 0.5f;
    public Transform puntoRefer;

    private float sigBala;

    void Update()
    {
        // Verificar tecla y cooldown
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= sigBala)
        {
            Shoot();
            sigBala = Time.time + coldown;
        }
    }

    void Shoot()
    {
        if (balaPrefab != null && puntoRefer != null)
        {
            Instantiate(balaPrefab, puntoRefer.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Faltan referencias en PlayerShooting");
        }
    }
}
