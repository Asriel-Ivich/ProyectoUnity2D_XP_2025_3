using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Referencias de Vida")]
    public Slider barraVida;
    public TextMeshProUGUI textoVida;
    public PlayerVida playerVida;

    [Header("Referencias de Tiempo")]
    public TextMeshProUGUI textoTemporizador;
    public CondicionDeVictoria condicionVictoria;

    [Header("Configuración")]
    public bool mostrarSegundos = true;
    public bool mostrarMilisegundos = false;

    void Start()
    {
        if (playerVida == null)
            playerVida = FindObjectOfType<PlayerVida>();

        if (condicionVictoria == null)
            condicionVictoria = FindObjectOfType<CondicionDeVictoria>();

        if (barraVida != null && playerVida != null)
        {
            barraVida.maxValue = playerVida.vidaMaxima;
            barraVida.value = playerVida.vidaActual;
        }

        ActualizarVidaUI();
        ActualizarTemporizadorUI();
    }

    void Update()
    {
        ActualizarVidaUI();
        ActualizarTemporizadorUI();
    }

    void ActualizarVidaUI()
    {
        if (playerVida == null) return;

        if (barraVida != null)
        {
            barraVida.value = playerVida.vidaActual;
        }

        if (textoVida != null)
        {
            textoVida.text = $"{playerVida.vidaActual}/{playerVida.vidaMaxima}";
        }
    }

    void ActualizarTemporizadorUI()
    {
        if (condicionVictoria == null || textoTemporizador == null) return;

        float tiempoRestante = condicionVictoria.GetCurrentTime();

        if (tiempoRestante <= 0)
        {
            textoTemporizador.text = "00:00";
            return;
        }

        string tiempoFormateado = FormatearTiempo(tiempoRestante);
        textoTemporizador.text = tiempoFormateado;

        if (tiempoRestante <= 30f) 
        {
            textoTemporizador.color = Color.yellow;
        }
        else if (tiempoRestante <= 59f) 
        {
            textoTemporizador.color = Color.red;
        }
        else
        {
            textoTemporizador.color = Color.white;
        }
    }

    string FormatearTiempo(float tiempoEnSegundos)
    {
        int minutos = Mathf.FloorToInt(tiempoEnSegundos / 60f);
        int segundos = Mathf.FloorToInt(tiempoEnSegundos % 60f);

        if (mostrarMilisegundos)
        {
            int milisegundos = Mathf.FloorToInt((tiempoEnSegundos * 1000) % 1000);
            return $"{minutos:00}:{segundos:00}.{milisegundos:000}";
        }
        else if (mostrarSegundos)
        {
            return $"{minutos:00}:{segundos:00}";
        }
        else
        {
            return $"{minutos:00}";
        }
    }

    public void OnPlayerVidaCambiada()
    {
        ActualizarVidaUI();
    }

    public void OnTiempoCambiado()
    {
        ActualizarTemporizadorUI();
    }
}
