using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

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

    public List<SpriteRenderer> listaSpriteRenderer = new List<SpriteRenderer>();
    public List<LayerNFT> listLayerNFTs = new List<LayerNFT>();

    int countId = 0;
    public List<string> listCodes = new List<string>();
    public List<string> listCodesDispoiveis = new List<string>();
    public List<int> listTotalForLayers = new List<int>();
    bool isGenerate = false;
    string code = "";
    // Start is called before the first frame update
    void Start()
    {
        
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
        }
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
                EditorUtility.DisplayDialog("Atenção!", "É obrigatório adicionar pelomenos uma camada!", "Ok");
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
                    listTotalForLayers.Clear();
                    listLayerNFTs.Clear();
                    listaSpriteRenderer.Clear();
                    EditorUtility.DisplayDialog("Atenção!", "É obrigatório preencher a camada com pelomenos uma imagem!", "Ok");
                }
                else
                {
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
                    listCodesDispoiveis.Shuffle();
                    isGenerate = true;
                }
            }                     
        }
        else
        {
            EditorUtility.DisplayDialog("Atenção!", "Preencha todos os campos!", "Ok");
        }

    }

    public void RandomImage()
    {
        while (true)
        {
            string codeSelect = listCodesDispoiveis.FirstOrDefault();
            if (codeSelect != "")
            {
                for(int i = 0; i<listLayerNFTs.Count; i++)
                {
                    listaSpriteRenderer[i].sprite = listLayerNFTs[i].listLayer[int.Parse(codeSelect[i].ToString())];
                }
                listCodes.Add(codeSelect);
                listCodesDispoiveis.Remove(codeSelect);
                break;
            }            
        }
        
    }

    private void LateUpdate()
    {
        if (isGenerate)
        {
            if (listCodes.Count() < pnlInteractionUser.maxNFTs)
            {
                TakeScreenShot(512, 512, listCodes.Count());
            }
        }            
    }

    [SerializeField] Camera cameraScreenShot;
    //WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public void TakeScreenShot(int resWidth, int resHeight, int id)
    {
        //yield return frameEnd; 
        Debug.Log("Take Screenshot");
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cameraScreenShot.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        cameraScreenShot.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        cameraScreenShot.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytesImg = screenShot.EncodeToPNG();
        string filename = ScreenShotName(id);
        System.IO.File.WriteAllBytes(filename, bytesImg);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
    }

    public string ScreenShotName(int id)
    {
        return string.Format("{0}/{1} #{2}.png",
                             pnlInteractionUser.urlFolder,
                             pnlInteractionUser.nameNFT,
                             id);
    }

    public void LoadTexture(Image imgTexture, byte[] bytesArray)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        tex.LoadRawTextureData(bytesArray);
        tex.Apply();
        imgTexture.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
    }
}
