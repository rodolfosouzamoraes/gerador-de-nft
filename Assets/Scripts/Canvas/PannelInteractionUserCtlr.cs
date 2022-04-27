using SFB;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe respons�vel por manipular as informa��es de entrada do usu�rio
/// </summary>
public class PannelInteractionUserCtlr : MonoBehaviour
{
    public string urlFolder;
    public int maxNFTs = 0;
    public string nameNFT;
    public InputField txtInputURL;
    public InputField txtInputQtdNFT;
    public InputField txtInputNameNFT;

    /// <summary>
    /// Seleciona a pasta onde ser� salva as NFTs
    /// </summary>
    public void SelectFolderToSaveNFT()
    {
        try
        {
            var paths = StandaloneFileBrowser.OpenFolderPanel("Select Folder", "", false);
            urlFolder = paths[0].Replace("\\", "/");
            txtInputURL.text = urlFolder; //urlFolder;
        }
        catch (Exception e)
        {
            Debug.Log("Houve um erro: "+e.Message);
        }
        
    }

    /// <summary>
    /// Define a vari�vel respons�vel por armazenar o m�ximo de NFTs a gerar.
    /// </summary>
    public void DefineMaxNFT()
    {
        int totalNFTInsertUser = int.Parse(txtInputQtdNFT.text);
        if (totalNFTInsertUser <= GameManager.Layers.countPossibilities)
        {
            maxNFTs = int.Parse(txtInputQtdNFT.text);
        }
        else
        {
            InsertMaxPossibilitiesInput();
        }
        
    }

    /// <summary>
    /// Insere o numero m�ximo de possibilidades de NFTs no campo.
    /// </summary>
    public void InsertMaxPossibilitiesInput()
    {
        maxNFTs = GameManager.Layers.countPossibilities;
        txtInputQtdNFT.text = "" + maxNFTs;
    }

    public void DefineNameNFT()
    {
        nameNFT = txtInputNameNFT.text;
    }
}
