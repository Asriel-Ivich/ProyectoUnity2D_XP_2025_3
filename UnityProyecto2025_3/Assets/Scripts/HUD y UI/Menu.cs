using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Configuración de Escena")]
    public string nombreEscenaJuego; 

    [Header("Referencias UI")]
    public GameObject panelConfiguraciones; 
    public Slider sliderVolumen; 

    private void Start()
    {
        if (panelConfiguraciones != null)
            panelConfiguraciones.SetActive(false);

        ConfigurarSliderVolumen();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelConfiguraciones != null && panelConfiguraciones.activeSelf)
            {
                CerrarConfiguraciones();
            }
        }
    }

    public void EmpezarJuego()
    {
        if (!string.IsNullOrEmpty(nombreEscenaJuego))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(nombreEscenaJuego);
        }
        else
        {
            Debug.LogError("No se ha asignado una escena");
        }
    }


    public void AbrirConfiguraciones()
    {
        if (panelConfiguraciones != null)
        {
            panelConfiguraciones.SetActive(true);
        }
    }

    public void CerrarConfiguraciones()
    {
        if (panelConfiguraciones != null)
        {
            panelConfiguraciones.SetActive(false);
        }
    }

    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

        // Para testing en el editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void CambiarVolumen(float volumen)
    {
        AudioListener.volume = volumen;
        PlayerPrefs.SetFloat("VolumenGlobal", volumen);
        Debug.Log("Volumen cambiado a: " + volumen);
    }

    private void ConfigurarSliderVolumen()
    {
        if (sliderVolumen != null)
        {
            float volumenGuardado = PlayerPrefs.GetFloat("VolumenGlobal", 1f);
            sliderVolumen.value = volumenGuardado;
            AudioListener.volume = volumenGuardado;

            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }
    }
}
