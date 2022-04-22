using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PannelTopCtlr : MonoBehaviour
{
    [SerializeField] Image imgAudioButton;
    [SerializeField] Image imgFullscreenButton;
    [SerializeField] Sprite audioOn;
    [SerializeField] Sprite audioOff;
    [SerializeField] Sprite fullscreenOn;
    [SerializeField] Sprite fullscreenOff;
    [SerializeField] AudioSource audioMusic;
    [SerializeField] AudioSource audioClickMouse;
    bool isFullScreen = false;

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
        GenerateNFT.Instance.pnlTutorial.SetActive(true);
    }

    public void OnOffFullScreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        imgFullscreenButton.sprite = isFullScreen == true ? fullscreenOn : fullscreenOff;
    }
}
