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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
}
