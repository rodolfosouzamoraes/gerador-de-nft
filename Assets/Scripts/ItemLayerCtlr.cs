using SFB;
using UnityEngine;
using UnityEngine.UI;

public class ItemLayerCtlr : MonoBehaviour
{
    public Text txtTotalImages;
    public Text txtNameLayer;
    LayerNFT layerNFT; // vai ter todas as imagens dessa layer 
    string[] paths;

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
        paths = StandaloneFileBrowser.OpenFilePanel("Overwrite with png", "", "png",true);
        GetImage();
    }

    private void GetImage()
    {
        if (paths != null)
        {
            foreach(string path in paths)
            {
                WWW www = new WWW("file:///" + path.Replace("\\","/"));
                if (www.texture.width != 1080 || www.texture.height != 1080)
                {
                    GameManager.DisplayDialog.ShowDialog("Atenção!", "Insira apenas imagens que possuem resolução 1080x1080.");
                    return;
                }
                Rect rect = new Rect(0, 0, www.texture.width, www.texture.height);
                Sprite spt = Sprite.Create(www.texture, rect, new Vector2(0.5f, 0.5f), 108);
                layerNFT.listLayer.Add(spt);
                txtTotalImages.text = layerNFT.listLayer.Count.ToString();
                GameManager.Layers.UpdatePossibilities();
            }
            
        }
    }
}
