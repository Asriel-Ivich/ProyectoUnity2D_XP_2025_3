using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Pausa : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelPausa;
    public Slider sliderVolumenPausa;
    public GameObject panelGameOver;
    public GameObject panelVictoria;
    public Image imagenGameOver;
    public Image imagenVictoria;

    [Header("Configuración de Escenas")]
    public string nombreEscenaMenu = "MenuPrincipal";
    public string nombreEscenaVictoria; 

    [Header("Tiempos de Espera")]
    public float tiempoEsperaGameOver = 3f;
    public float tiempoEsperaVictoria = 3f;

    private bool juegoPausado = false;
    private static float volumenGlobal = 1f;

    void Start()
    {
        volumenGlobal = PlayerPrefs.GetFloat("VolumenGlobal", 1f);
        AudioListener.volume = volumenGlobal;

        if (sliderVolumenPausa != null)
        {
            sliderVolumenPausa.value = volumenGlobal;
            sliderVolumenPausa.onValueChanged.AddListener(CambiarVolumenGlobal);
        }

        OcultarTodosLosPaneles();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !panelGameOver.activeSelf && !panelVictoria.activeSelf)
        {
            if (juegoPausado)
                ReanudarJuego();
            else
                PausarJuego();
        }
    }

    void OcultarTodosLosPaneles()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(false);
    }

    public void PausarJuego()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        if (panelPausa != null)
            panelPausa.SetActive(true);
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        if (panelPausa != null)
            panelPausa.SetActive(false);
    }

    public void CambiarVolumenGlobal(float nuevoVolumen)
    {
        volumenGlobal = nuevoVolumen;
        AudioListener.volume = volumenGlobal;
        PlayerPrefs.SetFloat("VolumenGlobal", nuevoVolumen);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(nombreEscenaMenu))
        {
            SceneManager.LoadScene(nombreEscenaMenu);
        }
    }

    public void MostrarGameOver()
    {
        Time.timeScale = 0f;
        juegoPausado = true;

        if (MusicaFondoController.Instance != null)
        {
            MusicaFondoController.Instance.DetenerMusica();
        }

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
            StartCoroutine(EsperarYVolverAlMenu(tiempoEsperaGameOver));
        }
    }

    public void MostrarVictoria()
    {
        Time.timeScale = 0f; 
        juegoPausado = true;

        if (MusicaFondoController.Instance != null)
        {
            MusicaFondoController.Instance.DetenerMusica();
        }

        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
            StartCoroutine(EsperarYIrAEscenaVictoria(tiempoEsperaVictoria));
        }
    }

    private IEnumerator EsperarYVolverAlMenu(float tiempoEspera)
    {
        yield return new WaitForSecondsRealtime(tiempoEspera);
        VolverAlMenu();
    }

    private IEnumerator EsperarYIrAEscenaVictoria(float tiempoEspera)
    {
        yield return new WaitForSecondsRealtime(tiempoEspera);
        IrAEscenaVictoria();
    }

    public void IrAEscenaVictoria()
    {
        Time.timeScale = 1f; 

        if (!string.IsNullOrEmpty(nombreEscenaVictoria))
        {
            SceneManager.LoadScene(nombreEscenaVictoria);
        }
        else
        {
            Debug.LogError("No se ha asignado un nombre de escena para la victoria");
            VolverAlMenu();
        }
    }

    public static float GetVolumenGlobal()
    {
        return volumenGlobal;
    }

    public bool EstaPausado()
    {
        return juegoPausado;
    }
}
