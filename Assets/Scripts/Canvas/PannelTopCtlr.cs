using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe responsável por controlar o painel do topo
/// </summary>
public class PannelTopCtlr : MonoBehaviour
{
    public Image imgAudioButton;
    public Image imgFullscreenButton;
    public Sprite audioOn;
    public Sprite audioOff;
    public Sprite fullscreenOn;
    public Sprite fullscreenOff;
    public AudioSource audioMusic;
    public AudioSource audioClickMouse;
    bool isFullScreen = false;

    private void Start()
    {
        isFullScreen = Screen.fullScreen;
        imgFullscreenButton.sprite = isFullScreen == true ? fullscreenOn : fullscreenOff;
    }

    void Update()
    {
        if (audioClickMouse.enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                audioClickMouse.Play();
            }
        }
    }

    /// <summary>
    /// Habilita e desabilita audios
    /// </summary>
    public void OnOrOffAudios()
    {
        audioMusic.enabled = !audioMusic.enabled;
        audioClickMouse.enabled = !audioClickMouse.enabled;
        imgAudioButton.sprite = audioMusic.enabled == true ? audioOn : audioOff;
    }

    /// <summary>
    /// Fecha a aplicação
    /// </summary>
    public void ExitApplication()
    {
        Application.Quit();
    }

    /// <summary>
    /// Exibe o tutorial
    /// </summary>
    public void ShowTutorial()
    {
        GameManager.Instance.pnlTutorial.SetActive(true);
    }

    /// <summary>
    /// Maximiza ou minima a tela do programa
    /// </summary>
    public void OnOffFullScreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        imgFullscreenButton.sprite = isFullScreen == true ? fullscreenOn : fullscreenOff;
    }
}
