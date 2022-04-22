using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class GenerateNFT : MonoBehaviour
{
    public static GenerateNFT Instance;
    public static PannelInteractionUserCtlr pnlInteractionUser;
    public static PannelLayersCtlr pnlLayers;
    private void Awake()
    {
        if(Instance == null)
        {
            pnlInteractionUser = GetComponent<PannelInteractionUserCtlr>();
            pnlLayers = GetComponent<PannelLayersCtlr>();
            Instance = this;
            return;
        }
        Destroy(this);
    }
    public GameObject goGenerateNFT;
    public List<SpriteRenderer> listaSpriteRenderer = new List<SpriteRenderer>();
    public List<LayerNFT> listLayerNFTs = new List<LayerNFT>();
    public List<string> listCodes = new List<string>();
    public List<string> listCodesDispoiveis = new List<string>();
    public List<int> listTotalForLayers = new List<int>();
    public Slider sldBar;
    public Text txtNameNFT;
    public DisplayDialogCtlr displayDialog;
    public GameObject pnlQuestionTutorial;
    public GameObject pnlTutorial;
    bool isGenerate = false;
    string code = "";
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("Tutorial") == 0)
        {
            pnlQuestionTutorial.SetActive(true);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (isGenerate)
        {
            if (listCodes.Count() < pnlInteractionUser.maxNFTs)
            {
                RandomImage();
            }
            else
            {
                StopGenerate();
            }
        }
    }

    public void ClearLists()
    {
        listTotalForLayers.Clear();
        listLayerNFTs.Clear();
        listaSpriteRenderer.Clear();
        listCodesDispoiveis.Clear();
        listCodes.Clear();
    }

    //Uma maneira de percorer todos os elementos da lista
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
            listCodesDispoiveis.Add(code);
        }
    }

    public void GenerateImagesNFT()
    {
        if(pnlInteractionUser.txtInputURL.text!="" && pnlInteractionUser.txtInputQtdNFT.text!="" && pnlInteractionUser.txtInputNameNFT.text != "")
        {
            listLayerNFTs = pnlLayers.LayersNFT;
            listaSpriteRenderer = pnlLayers.RenderesSprite;
            if(listLayerNFTs.Count == 0)
            {
                displayDialog.ShowDialog("Atenção!", "É obrigatório adicionar pelomenos uma camada!");
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
                    displayDialog.ShowDialog("Atenção!", "É obrigatório preencher a camada com pelomenos uma imagem!");
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

                    pnlInteractionUser.DefineMaxNFT();
                    pnlInteractionUser.DefineNameNFT();
                    List<string> result = listCodesDispoiveis.Except(listCodes).ToList(); // remove todos os codigos que já foram utilizados
                    listCodesDispoiveis = result;
                    listCodesDispoiveis.Shuffle();
                    goGenerateNFT.SetActive(true);
                    isGenerate = true;                    
                }
            }                     
        }
        else
        {
            displayDialog.ShowDialog("Atenção!", "Preencha todos os campos!");
        }

    }

    public void StopGenerate()
    {
        ClearLists();
        isGenerate = false;
        goGenerateNFT.SetActive(false);
        Application.OpenURL(pnlInteractionUser.urlFolder);
    }

    public void RandomImage()
    {
        OpenFileWithCodes();
        while (true)
        {
            string codeSelect = listCodesDispoiveis.FirstOrDefault();
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
                listCodes.Add(codeSelect);
                listCodesDispoiveis.Remove(codeSelect);
                SaveCodesInFile();
                break;
            }
            return;
        }        
    }

    private void LateUpdate()
    {
        if (isGenerate)
        {
            if (listCodes.Count() <= pnlInteractionUser.maxNFTs)
            {
                TakeScreenShot(1080, 1080, listCodes.Count(), listCodes.LastOrDefault());
                sldBar.value =(float)listCodes.Count() / (float)pnlInteractionUser.maxNFTs;
            }
            //listCodes.Clear();
            GameManager.IncrementCountNFT();
        }
        
    }

    [SerializeField] Camera cameraScreenShot;
    //WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public void TakeScreenShot(int resWidth, int resHeight, int id, string code)
    {
        //yield return frameEnd; 
        //Debug.Log("Take Screenshot");
        RenderTexture rt = cameraScreenShot.targetTexture;
        //cameraScreenShot.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cameraScreenShot.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        //cameraScreenShot.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        //Destroy(rt);
        byte[] bytesImg = screenShot.EncodeToPNG();
        string filename = ScreenShotName(id, code);
        txtNameNFT.text = $"{pnlInteractionUser.nameNFT} #{id}";
        File.WriteAllBytes(filename, bytesImg);
        //Debug.Log(string.Format("Took screenshot to: {0}", filename));
        //imgNFT.sprite = LoadTexture(bytesImg);
    }

    public string ScreenShotName(int id, string code)
    {
        return string.Format("{0}/{1} #{2} - {3}.png",
                             pnlInteractionUser.urlFolder,
                             pnlInteractionUser.nameNFT,
                             id,
                             code);
    }

    public Sprite LoadTexture(byte[] bytesArray)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        tex.LoadRawTextureData(bytesArray);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
    }

    public void SaveCodesInFile()
    {
        FileStream fs = new FileStream("save.dat", FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, listCodes);
        fs.Close();
    }

    public void OpenFileWithCodes()
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

    public void SetViewTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        pnlQuestionTutorial.SetActive(false);
    }

    public void ViewTutorial()
    {
        SetViewTutorial();
        pnlTutorial.SetActive(true);
    }
}
