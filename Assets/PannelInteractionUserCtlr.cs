using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PannelInteractionUserCtlr : MonoBehaviour
{
    public string urlFolder;
    public int maxNFTs = 0;
    public string nameNFT;
    public InputField txtInputURL;
    public InputField txtInputQtdNFT;
    public InputField txtInputNameNFT;
    public void SelectFolderToSaveNFT()
    {
        urlFolder = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        txtInputURL.text = urlFolder;
    }

    public void DefineMaxNFT()
    {
        maxNFTs = int.Parse(txtInputQtdNFT.text);
    }

    public void DefineNameNFT()
    {
        nameNFT = txtInputNameNFT.text;
    }
}
