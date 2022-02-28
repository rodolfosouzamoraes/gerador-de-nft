using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemLayerCtlr : MonoBehaviour
{
    public Text txtTotalImages;
    public Text txtNameLayer;
    LayerNFT layerNFT; // vai ter todas as imagens dessa layer 
    string path;

    public LayerNFT LayerNFT
    {
        get { return layerNFT; }
        private set { }
    }
    // Start is called before the first frame update
    void Start()
    {
        layerNFT = new LayerNFT();
    }

    public void NameLayer(int number)
    {
        txtNameLayer.text = "Camada " + number;
    }

    public void SelectImage()
    {
        path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
        GetImage();
    }

    private void GetImage()
    {
        if (path != null)
        {
            
            WWW www = new WWW("file:///" + path);
            Rect rect = new Rect(0,0,www.texture.width,www.texture.height);
            Sprite spt = Sprite.Create(www.texture, rect, new Vector2(0.5f, 0.5f), 638);
            layerNFT.listLayer.Add(spt);
            //spriteRenderer.sprite = layerNFT.listLayer[0];
            txtTotalImages.text = layerNFT.listLayer.Count.ToString();
            GenerateNFT.pnlLayers.UpdatePossibilities();
        }
    }
}
