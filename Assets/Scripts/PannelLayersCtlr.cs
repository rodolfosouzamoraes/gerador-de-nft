using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PannelLayersCtlr : MonoBehaviour
{
    public GameObject itemLayer;
    public GameObject layerRenderer;
    public Transform contentLayers;
    public Text txtTotalLayer;
    public Text txtTotalPossibilities;

    int countLayers = 0;
    int countPossibilities = 0;
    List<GameObject> listItensContent = new List<GameObject>();
    List<GameObject> listLayerRendererContent = new List<GameObject>();

    public List<LayerNFT> LayersNFT
    {
        get
        {
            List<LayerNFT> list = new List<LayerNFT>();
            foreach(GameObject item in listItensContent)
            {
                list.Add(item.GetComponent<ItemLayerCtlr>().LayerNFT);
            }
            return list;
        }
        private set { }
    }

    public List<SpriteRenderer> RenderesSprite
    {
        get
        {
            List<SpriteRenderer> list = new List<SpriteRenderer>();
            foreach(GameObject item in listLayerRendererContent)
            {
                list.Add(item.GetComponent<SpriteRenderer>());
            }
            return list;
        }
    }

    private void Start()
    {
        txtTotalLayer.text =""+ countLayers;
        txtTotalPossibilities.text = "" + countPossibilities;
    }
    public void AddNewLayer()
    {
        GameObject item = Instantiate(itemLayer, contentLayers);        
        GameObject itemLayerRenderer = Instantiate(layerRenderer, null);

        listItensContent.Add(item);
        listLayerRendererContent.Add(itemLayerRenderer);

        IncrementCountLayers();
    }

    public void RemoveLayer()
    {
        if (listItensContent.Count > 0)
        {
            GameObject lastItem = listItensContent.LastOrDefault();
            listItensContent.Remove(lastItem);
            Destroy(lastItem);

            GameObject lastItemLayerRender = listLayerRendererContent.LastOrDefault();
            listLayerRendererContent.Remove(lastItemLayerRender);
            Destroy(lastItemLayerRender);

            DecrementCountLayer();

        }
    }

    private void IncrementCountLayers()
    {
        countLayers++;
        txtTotalLayer.text = "" + countLayers;
    }

    private void DecrementCountLayer()
    {
        countLayers--;
        txtTotalLayer.text = "" + countLayers;
    }


}
