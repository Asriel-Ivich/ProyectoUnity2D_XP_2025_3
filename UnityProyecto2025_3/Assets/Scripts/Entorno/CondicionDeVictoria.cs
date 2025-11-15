using UnityEngine;
using UnityEngine.SceneManagement;

public class CondicionDeVictoria : MonoBehaviour
{
    [SerializeField] private float tiempo = 240f; //4 minutos
    private bool final = false;

    void Update()
    {
        if (final) return;

        tiempo -= Time.deltaTime;

        // Verificar si el tiempo se ha agotado
        if (tiempo <= 0f)
        {
            FinDelJuego();
        }
    }

    void FinDelJuego()
    {
        final = true;
        Debug.Log("¡Se acabo! Lograste Aguantar.");

        // Cerrar la aplicación (solo en build)
        Application.Quit();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public float GetCurrentTime()
    {
        return tiempo;
    }
}
