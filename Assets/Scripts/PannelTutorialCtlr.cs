using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PannelTutorialCtlr : MonoBehaviour
{

    public void CloseTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        gameObject.SetActive(false);
    }
}
