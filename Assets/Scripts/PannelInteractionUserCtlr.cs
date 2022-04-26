using SFB;
using System;
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
        try
        {
            var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", false);//EditorUtility.OpenFolderPanel("Select Directory", "", "");
            urlFolder = paths[0].Replace("\\", "/");
            txtInputURL.text = urlFolder; //urlFolder;
        }
        catch (Exception e)
        {
            Debug.Log("Houve um erro: "+e.Message);
        }
        
    }

    public void DefineMaxNFT()
    {
        int totalNFTInsertUser = int.Parse(txtInputQtdNFT.text);
        if (totalNFTInsertUser <= GameManager.pnlLayers.countPossibilities)
        {
            maxNFTs = int.Parse(txtInputQtdNFT.text);
        }
        else
        {
            InsertMaxPossibilitiesInput();
        }
        
    }

    public void InsertMaxPossibilitiesInput()
    {
        maxNFTs = GameManager.pnlLayers.countPossibilities;
        txtInputQtdNFT.text = "" + maxNFTs;
    }

    public void DefineNameNFT()
    {
        nameNFT = txtInputNameNFT.text;
    }

    public void OpenFolder()
    {
        
    }
}
