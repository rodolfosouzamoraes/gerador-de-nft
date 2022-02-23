using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    [SerializeField] SpriteRenderer camada1_background;
    [SerializeField] SpriteRenderer camada2_calca;
    [SerializeField] SpriteRenderer camada3_corpo;
    [SerializeField] SpriteRenderer camada4_camisa;
    [SerializeField] SpriteRenderer camada5_boca;
    [SerializeField] SpriteRenderer camada6_olho;
    [SerializeField] SpriteRenderer camada7_bone;

    [SerializeField] List<Sprite> listaBackground;
    [SerializeField] List<Sprite> listaCalca;
    [SerializeField] List<Sprite> listaCorpo;
    [SerializeField] List<Sprite> listaCamisa;
    [SerializeField] List<Sprite> listaBoca;
    [SerializeField] List<Sprite> listaOlho;
    [SerializeField] List<Sprite> listaBone;

    int countId = 0;
    public List<string> listCodes = new List<string>();
    public List<string> listCodesDispoiveis = new List<string>();
    bool isGenerate = false;
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

    public void GenerateImagesNFT()
    {
        pnlInteractionUser.DefineMaxNFT();
        pnlInteractionUser.DefineNameNFT();
        if(pnlInteractionUser.maxNFTs>0 && pnlInteractionUser.nameNFT != "" && pnlInteractionUser.urlFolder != "")
        {
            int count = 0;
            //Tentar inserir na lista o total de imagens por lista de camadas, ou seja, cada camada vai ter x tamanho de imagens, armazenar esse valor para poder montar as possibilidades.
            for (int i1 = 0; i1 < listaBackground.Count; i1++)
            {
                for (int i2 = 0; i2 < listaCalca.Count; i2++)
                {
                    for (int i3 = 0; i3 < listaCorpo.Count; i3++)
                    {
                        for (int i4 = 0; i4 < listaCamisa.Count; i4++)
                        {
                            for (int i5 = 0; i5 < listaBoca.Count; i5++)
                            {
                                for (int i6 = 0; i6 < listaOlho.Count; i6++)
                                {
                                    for (int i7 = 0; i7 < listaBone.Count; i7++)
                                    {
                                        string code = "" + i1 + "" + i2 + "" + i3 + "" + i4 + "" + i5 + "" + i6 + "" + i7;
                                        listCodesDispoiveis.Add(code);
                                        count++;
                                        /*if (count > maxNFTs)
                                        {
                                            //return;
                                        }*/
                                    }
                                }
                            }
                        }
                    }
                }
            }
            listCodesDispoiveis.Shuffle();
            if (pnlInteractionUser.maxNFTs == -1)
            {
                pnlInteractionUser.maxNFTs = listCodesDispoiveis.Count();
            }
            isGenerate = true;
        }
        
    }

    public void RandomImage()
    {
        while (true)
        {
            string codeSelect = listCodesDispoiveis.FirstOrDefault();
            if (codeSelect != "")
            {
                camada1_background.sprite = listaBackground[int.Parse(codeSelect[0].ToString())];
                camada2_calca.sprite = listaCalca[int.Parse(codeSelect[1].ToString())];
                camada3_corpo.sprite = listaCorpo[int.Parse(codeSelect[2].ToString())];
                camada4_camisa.sprite = listaCamisa[int.Parse(codeSelect[3].ToString())];
                camada5_boca.sprite = listaBoca[int.Parse(codeSelect[4].ToString())];
                camada6_olho.sprite = listaOlho[int.Parse(codeSelect[5].ToString())];
                camada7_bone.sprite = listaBone[int.Parse(codeSelect[6].ToString())];
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
    private bool isTakeScreenShotOneNextFrame;
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
