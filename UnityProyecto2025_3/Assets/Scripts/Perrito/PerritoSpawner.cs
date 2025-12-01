using UnityEngine;
using System.Collections;

public class PerritoSpawner : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject dogPrefab;
    public float tiempoEntreSpawn = 5f;
    public int maxPerrosEnEscena = 3;
    public LayerMask layerPisos;

    private int perrosActuales = 0;
    private Collider2D[] pisosEnEscena;

    void Start()
    {
        EncontrarPisos();
        StartCoroutine(SpawnPerrosCorrutina());
    }

    void EncontrarPisos()
    {
        // Encontrar todos los colliders con las layers correctas
        Collider2D[] todosLosColliders = FindObjectsOfType<Collider2D>();
        System.Collections.Generic.List<Collider2D> pisosList = new System.Collections.Generic.List<Collider2D>();

        foreach (Collider2D collider in todosLosColliders)
        {
            if (((1 << collider.gameObject.layer) & layerPisos.value) != 0)
            {
                pisosList.Add(collider);
            }
        }

        pisosEnEscena = pisosList.ToArray();
        Debug.Log($"Se encontraron {pisosEnEscena.Length} pisos en la escena");
    }

    IEnumerator SpawnPerrosCorrutina()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreSpawn);

            if (perrosActuales < maxPerrosEnEscena && pisosEnEscena.Length > 0)
            {
                SpawnPerro();
            }
        }
    }

    void SpawnPerro()
    {
        // Elegir un piso aleatorio
        Collider2D pisoAleatorio = pisosEnEscena[Random.Range(0, pisosEnEscena.Length)];

        // Calcular posición segura en el piso
        Vector2 posicionSpawn = CalcularPosicionSeguraEnPiso(pisoAleatorio);

        // Instanciar el perro
        GameObject nuevoPerro = Instantiate(dogPrefab, posicionSpawn, Quaternion.identity);

        // Configurar el script del perro
        MovimientoPerrito dogScript = nuevoPerro.GetComponent<MovimientoPerrito>();
        if (dogScript != null)
        {
            dogScript.SetSpawner(this);
        }

        perrosActuales++;
        Debug.Log($"Perro instanciado. Total perros: {perrosActuales}");
    }

    Vector2 CalcularPosicionSeguraEnPiso(Collider2D piso)
    {
        Bounds bounds = piso.bounds;

        // Calcular posición aleatoria en el centro del piso (no cerca de los bordes)
        float margenSeguro = 0.5f; // Margen de seguridad desde los bordes
        float xMin = bounds.min.x + margenSeguro;
        float xMax = bounds.max.x - margenSeguro;

        // Asegurarse de que el mínimo sea menor que el máximo
        if (xMin >= xMax)
        {
            xMin = bounds.center.x - 0.1f;
            xMax = bounds.center.x + 0.1f;
        }

        float x = Random.Range(xMin, xMax);
        float y = bounds.max.y + 0.1f; // Un poco por encima del piso

        return new Vector2(x, y);
    }

    public void PerroDestruido()
    {
        perrosActuales--;
        perrosActuales = Mathf.Max(0, perrosActuales);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (pisosEnEscena != null)
        {
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