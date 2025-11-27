using UnityEngine;
using System.Collections;

public class PerritoSpawner : MonoBehaviour
{
    [Header("Configuración del Item")]
    public GameObject perritoPrefab;
    public float tiempoEntreSpawn = 10f;
    public int maxItemsEnEscena = 3;

    [Header("Configuración de Layer")]
    public LayerMask layerPiso;

    private int itemsActuales = 0;
    private Collider2D[] pisosEnEscena;

    void Start()
    {
        EncontrarPisos();

        StartCoroutine(SpawnItemsCorrutina());
    }

    void EncontrarPisos()
    {
        pisosEnEscena = Physics2D.OverlapAreaAll(
            new Vector2(-100, -100),
            new Vector2(100, 100),
            layerPiso
        );

        Debug.Log($"Se encontraron {pisosEnEscena.Length} pisos en la escena");
    }

    IEnumerator SpawnItemsCorrutina()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreSpawn);

            if (itemsActuales < maxItemsEnEscena && pisosEnEscena.Length > 0)
            {
                SpawnItem();
            }
        }
    }

    void SpawnItem()
    {
        Collider2D pisoAleatorio = pisosEnEscena[Random.Range(0, pisosEnEscena.Length)];

        Vector2 posicionSpawn = CalcularPosicionEnPiso(pisoAleatorio);

        GameObject nuevoItem = Instantiate(perritoPrefab, posicionSpawn, Quaternion.identity);

        MovimientoPerrito itemMovement = nuevoItem.GetComponent<MovimientoPerrito>();
        if (itemMovement != null)
        {
            itemMovement.SetSpawner(this);
        }

        itemsActuales++;
        Debug.Log($"Item instanciado. Total items: {itemsActuales}");
    }

    Vector2 CalcularPosicionEnPiso(Collider2D piso)
    {
        Bounds bounds = piso.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.max.y;

        return new Vector2(x, y);
    }

    public void ItemDestruido()
    {
        itemsActuales--;
        itemsActuales = Mathf.Max(0, itemsActuales); 
    }

    void OnDrawGizmosSelected()
    {
        if (pisosEnEscena != null)
        {
            Gizmos.color = Color.green;
            foreach (Collider2D piso in pisosEnEscena)
            {
                if (piso != null)
                {
                    Bounds bounds = piso.bounds;
                    Gizmos.DrawWireCube(bounds.center, bounds.size);
                }
            }
        }
    }
}
