using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PannelTopCtlr : MonoBehaviour
{
    [SerializeField] Image imgAudioButton;
    [SerializeField] Sprite audioOn;
    [SerializeField] Sprite audioOff;
    [SerializeField] AudioSource audioMusic;
    [SerializeField] AudioSource audioClickMouse;

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
}
