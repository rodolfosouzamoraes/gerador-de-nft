using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PannelInteractionUserCtlr : MonoBehaviour
{
    public static PannelInteractionUserCtlr Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(this);
    }

    public string urlFolder;
    public InputField txtInputURL;
    public void SelectFolderToSaveNFT()
    {
        urlFolder = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        txtInputURL.text = urlFolder;
    }
}
