using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

/// <summary>
/// Classe responsável por gerar as NFTs e salva-las no computador.
/// </summary>
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
    private int countNFTs = 0;

    void Update()
    {
        if (isGenerate)
        {
            if (listCodes.Count() < GameManager.InteractionUser.maxNFTs)
            {
                RandomImage();
            }
            else
            {
                StopGenerate();
            }
        }
    }

    /// <summary>
    /// Limpa as listas que fazem parte da lógica de gerar as NFTs
    /// </summary>
    private void ClearLists()
    {
        listTotalForLayers.Clear();
        listLayerNFTs.Clear();
        listaSpriteRenderer.Clear();
        listCodesAvailable.Clear();
        listCodes.Clear();
    }

    /// <summary>
    /// Gera um código para cada combinação de camadas.
    /// No final a string será formada por todos os valores de códigos das layers
    /// </summary>
    /// <param name="index">Indice do código da camada.</param>
    /// <param name="idPrevious">O indice anterior do código.</param>
    private void GenerateCodes(int index, int idPrevious)
    {
        code += ""+ idPrevious; // adiciono o proximo id no CODE
        if (index < listTotalForLayers.Count) // vou até o ultimo index da listTotalForLayers
        {
            for (int i = 0; i < listTotalForLayers[index]; i++)
            {
                GenerateCodes(index + 1, i); // acesso o mesmo método para poder descer mais um degrau da lista.
                string recoverCode = code.Remove(code.Length - 1, 1); // removo o id anterior para poder inserir o novo no próximo.
                code = recoverCode;
            }
        }
        else
        {
            listCodesAvailable.Add(code);
        }
    }

    /// <summary>
    /// Gera as combinações de imagens
    /// </summary>
    public void GenerateImagesNFT()
    {
        if(GameManager.InteractionUser.txtInputURL.text!="" && GameManager.InteractionUser.txtInputQtdNFT.text!="" && GameManager.InteractionUser.txtInputNameNFT.text != "")
        {
            listLayerNFTs = GameManager.Layers.LayersNFT;
            listaSpriteRenderer = GameManager.Layers.RenderesSprite;
            if(listLayerNFTs.Count == 0)
            {
                GameManager.DisplayDialog.ShowDialog("Atenção!", "É obrigatório adicionar pelomenos uma camada!");
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
                    GameManager.DisplayDialog.ShowDialog("Atenção!", "É obrigatório preencher a camada com pelomenos uma imagem!");
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

                    GameManager.InteractionUser.DefineMaxNFT();
                    GameManager.InteractionUser.DefineNameNFT();
                    List<string> result = listCodesAvailable.Except(listCodes).ToList(); // remove todos os codigos que já foram utilizados
                    listCodesAvailable = result;
                    listCodesAvailable.Shuffle();
                    pnlGenerateNFT.SetActive(true);
                    isGenerate = true;                    
                }
            }                     
        }
        else
        {
            GameManager.DisplayDialog.ShowDialog("Atenção!", "Preencha todos os campos!");
        }

    }

    public void StopGenerate()
    {
        ClearLists();
        isGenerate = false;
        pnlGenerateNFT.SetActive(false);
        Application.OpenURL(GameManager.InteractionUser.urlFolder);
    }

    /// <summary>
    /// Randomiza a lista de códigos para poder ser gerado aleatóriamente.
    /// </summary>
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

    /// <summary>
    /// Transfere o código disponivel para a lista de codigos que já geraram o NFT
    /// </summary>
    /// <param name="codeSelect"></param>
    private void ChangeListCode(string codeSelect)
    {
        listCodes.Add(codeSelect);
        listCodesAvailable.Remove(codeSelect);
    }

    /// <summary>
    /// O LateUpdate tem a funcionalidade chamar o método que irá tirar o print da tela atrávez da camera
    /// </summary>
    private void LateUpdate()
    {
        if (isGenerate)
        {
            if (listCodes.Count() <= GameManager.InteractionUser.maxNFTs)
            {
                TakeScreenShot(1080, 1080, listCodes.Count(), listCodes.LastOrDefault());
                UpdateProgressBar();
            }
            IncrementCountNFT();
        }
        
    }

    /// <summary>
    /// Atualiza o Slide Bar da tela de geração
    /// </summary>
    private void UpdateProgressBar()
    {
        sldBar.value = (float)listCodes.Count() / (float)GameManager.InteractionUser.maxNFTs;
    }

    /// <summary>
    /// Tira um print da imagem gerada a frente da camera da cena.
    /// </summary>
    /// <param name="resWidth">Largura da imagem</param>
    /// <param name="resHeight">Altura da imagem</param>
    /// <param name="id">Id da imagem</param>
    /// <param name="code">Codigo da imagem</param>
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
        txtNameNFT.text = $"{GameManager.InteractionUser.nameNFT} #{id}";
        File.WriteAllBytes(filename, bytesImg);
    }

    /// <summary>
    /// Da um nome ao print da imagem.
    /// </summary>
    /// <param name="id">Id da imagem</param>
    /// <param name="code">Codigo da imagem</param>
    /// <returns></returns>
    private string ScreenShotName(int id, string code)
    {
        return string.Format("{0}/{1} #{2} - {3}.png",
                             GameManager.InteractionUser.urlFolder,
                             GameManager.InteractionUser.nameNFT,
                             id,
                             code);
    }

    /// <summary>
    /// Salva a lista de codigos gerados em um arquivo
    /// </summary>
    private void SaveCodesInFile()
    {
        FileStream fs = new FileStream("save.dat", FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, listCodes);
        fs.Close();
    }

    /// <summary>
    /// Abre a lista de códigos já gerados salvos num arquivo.
    /// </summary>
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

    /// <summary>
    /// Incrementa a contagem de NFTs gerados.
    /// </summary>
    public void IncrementCountNFT()
    {
        countNFTs++;
        if (countNFTs > 50)
        {
            countNFTs = 0;
            GameSceneManager.ReloadScene();
        }
    }
}
