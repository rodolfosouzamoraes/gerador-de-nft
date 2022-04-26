using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class GenerateNFT : MonoBehaviour
{
    public GameObject pnlGenerateNFT;
    public Slider sldBar;
    public Text txtNameNFT;
    public Camera cameraScreenShot;

    private List<SpriteRenderer> listaSpriteRenderer = new List<SpriteRenderer>();
    private List<LayerNFT> listLayerNFTs = new List<LayerNFT>();
    private List<string> listCodes = new List<string>();
    private List<string> listCodesAvailable = new List<string>();
    private List<int> listTotalForLayers = new List<int>();
    private bool isGenerate = false;
    private string code = "";

    void Update()
    {
        if (isGenerate)
        {
            if (listCodes.Count() < GameManager.pnlInteractionUser.maxNFTs)
            {
                RandomImage();
            }
            else
            {
                StopGenerate();
            }
        }
    }

    private void ClearLists()
    {
        listTotalForLayers.Clear();
        listLayerNFTs.Clear();
        listaSpriteRenderer.Clear();
        listCodesAvailable.Clear();
        listCodes.Clear();
    }

    //Uma maneira de percorer todos os elementos da lista
    private void GenerateCodes(int index, int idPrevious)
    {
        code += ""+ idPrevious; // adiciono o proximo id no CODE
        if (index < listTotalForLayers.Count) // vou at� o ultimo index da listTotalForLayers
        {
            for (int i = 0; i < listTotalForLayers[index]; i++)
            {
                GenerateCodes(index + 1, i); // acesso o mesmo m�todo para poder descer mais um degrau da lista.
                string recoverCode = code.Remove(code.Length - 1, 1); // removo o id anterior para poder inserir o novo no pr�ximo.
                code = recoverCode;
            }
        }
        else
        {
            listCodesAvailable.Add(code);
        }
    }

    public void GenerateImagesNFT()
    {
        if(GameManager.pnlInteractionUser.txtInputURL.text!="" && GameManager.pnlInteractionUser.txtInputQtdNFT.text!="" && GameManager.pnlInteractionUser.txtInputNameNFT.text != "")
        {
            listLayerNFTs = GameManager.pnlLayers.LayersNFT;
            listaSpriteRenderer = GameManager.pnlLayers.RenderesSprite;
            if(listLayerNFTs.Count == 0)
            {
                GameManager.pnlDisplayDialog.ShowDialog("Aten��o!", "� obrigat�rio adicionar pelomenos uma camada!");
            }
            else
            {
                bool isZero = false;
                foreach (LayerNFT layer in listLayerNFTs)
                {
                    if (layer.listLayer.Count == 0)
                    {
                        isZero = true;
                        break;
                    }
                    listTotalForLayers.Add(layer.listLayer.Count);
                }

                if (isZero)
                {
                    ClearLists();
                    GameManager.pnlDisplayDialog.ShowDialog("Aten��o!", "� obrigat�rio preencher a camada com pelomenos uma imagem!");
                }
                else
                {
                    OpenFileWithCodes();
                    for (int i = 0; i < listTotalForLayers[0]; i++)
                    {
                        GenerateCodes(1, i);
                        code = "";
                    }

                    int countOrderLayerSpriteRenderer = 0;
                    foreach (SpriteRenderer spt in listaSpriteRenderer)
                    {
                        spt.sortingOrder = countOrderLayerSpriteRenderer;
                        countOrderLayerSpriteRenderer++;
                    }

                    GameManager.pnlInteractionUser.DefineMaxNFT();
                    GameManager.pnlInteractionUser.DefineNameNFT();
                    List<string> result = listCodesAvailable.Except(listCodes).ToList(); // remove todos os codigos que j� foram utilizados
                    listCodesAvailable = result;
                    listCodesAvailable.Shuffle();
                    pnlGenerateNFT.SetActive(true);
                    isGenerate = true;                    
                }
            }                     
        }
        else
        {
            GameManager.pnlDisplayDialog.ShowDialog("Aten��o!", "Preencha todos os campos!");
        }

    }

    public void StopGenerate()
    {
        ClearLists();
        isGenerate = false;
        pnlGenerateNFT.SetActive(false);
        Application.OpenURL(GameManager.pnlInteractionUser.urlFolder);
    }

    private void RandomImage()
    {
        OpenFileWithCodes();
        while (true)
        {
            string codeSelect = listCodesAvailable.FirstOrDefault();
            if (codeSelect != "" || codeSelect != null)
            {
                for(int i = 0; i<listLayerNFTs.Count; i++)
                {
                    try
                    {
                        listaSpriteRenderer[i].sprite = listLayerNFTs[i].listLayer[int.Parse(codeSelect[i].ToString())];
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("Erro: " + ex.Message);
                        StopGenerate();
                        return;
                    }
                }
                ChangeListCode(codeSelect);
                SaveCodesInFile();
                break;
            }
            return;
        }        
    }

    private void ChangeListCode(string codeSelect)
    {
        listCodes.Add(codeSelect);
        listCodesAvailable.Remove(codeSelect);
    }

    private void LateUpdate()
    {
        if (isGenerate)
        {
            if (listCodes.Count() <= GameManager.pnlInteractionUser.maxNFTs)
            {
                TakeScreenShot(1080, 1080, listCodes.Count(), listCodes.LastOrDefault());
                UpdateProgressBar();
            }
            GameSceneManager.IncrementCountNFT();
        }
        
    }

    private void UpdateProgressBar()
    {
        sldBar.value = (float)listCodes.Count() / (float)GameManager.pnlInteractionUser.maxNFTs;
    }

    private void TakeScreenShot(int resWidth, int resHeight, int id, string code)
    {
        RenderTexture rt = cameraScreenShot.targetTexture;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cameraScreenShot.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        RenderTexture.active = null;
        byte[] bytesImg = screenShot.EncodeToPNG();
        string filename = ScreenShotName(id, code);
        txtNameNFT.text = $"{GameManager.pnlInteractionUser.nameNFT} #{id}";
        File.WriteAllBytes(filename, bytesImg);
    }

    private string ScreenShotName(int id, string code)
    {
        return string.Format("{0}/{1} #{2} - {3}.png",
                             GameManager.pnlInteractionUser.urlFolder,
                             GameManager.pnlInteractionUser.nameNFT,
                             id,
                             code);
    }

    private void SaveCodesInFile()
    {
        FileStream fs = new FileStream("save.dat", FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, listCodes);
        fs.Close();
    }

    private void OpenFileWithCodes()
    {
        using (Stream stream = File.Open("save.dat", FileMode.OpenOrCreate))
        {
            if (stream.Length > 0)
            {
                var bformatter = new BinaryFormatter();

                listCodes = (List<string>)bformatter.Deserialize(stream);
            }            
        }
    }    
}
