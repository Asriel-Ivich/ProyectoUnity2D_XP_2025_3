using UnityEngine;
using UnityEngine.SceneManagement;

public class CondicionDeVictoria : Pausa
{
    [SerializeField] private float tiempo = 240f; //4 minutos
    private bool final = false;

    public System.Action<float> OnTiempoCambiado;

    void Update()
    {
        if (final) return;

        float tiempoAnterior = tiempo;
        tiempo -= Time.deltaTime;
        tiempo = Mathf.Max(0, tiempo);

        if (Mathf.FloorToInt(tiempoAnterior) != Mathf.FloorToInt(tiempo))
        {
            OnTiempoCambiado?.Invoke(tiempo);
        }

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

        MostrarVictoria();
    }

    public float GetCurrentTime()
    {
        return tiempo;
    }
}
