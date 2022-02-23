using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PannelInteractionUserCtlr : MonoBehaviour
{
    public string urlFolder;
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
    public void SelectFolderToSaveNFT()
    {
        string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        urlFolder = directory;
    }
}
