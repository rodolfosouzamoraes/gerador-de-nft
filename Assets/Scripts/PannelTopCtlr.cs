using UnityEngine;
using UnityEngine.UI;

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

    public void OnOrOffAudios()
    {
        audioMusic.enabled = !audioMusic.enabled;
        audioClickMouse.enabled = !audioClickMouse.enabled;
        imgAudioButton.sprite = audioMusic.enabled == true ? audioOn : audioOff;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void ShowTutorial()
    {
        GameManager.Instance.pnlTutorial.SetActive(true);
    }

    public void OnOffFullScreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        imgFullscreenButton.sprite = isFullScreen == true ? fullscreenOn : fullscreenOff;
    }
}
