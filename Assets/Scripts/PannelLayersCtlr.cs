using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PannelLayersCtlr : MonoBehaviour
{
    public List<LayerNFT> listLayerNFT = new List<LayerNFT>();
    public GameObject itemLayer;
    public Transform contentLayers;
    public Text txtTotalLayer;
    public Text txtTotalPossibilities;

    int countLayers = 0;
    int countPossibilities = 0;
    List<GameObject> listItensContent = new List<GameObject>();

    private void Start()
    {
        txtTotalLayer.text =""+ countLayers;
        txtTotalPossibilities.text = "" + countPossibilities;
    }
    public void AddNewLayer()
    {
        LayerNFT layerNFT = new LayerNFT();
        listLayerNFT.Add(layerNFT);
        GameObject item = Instantiate(itemLayer, contentLayers);
        listItensContent.Add(item);
        IncrementCountLayers();
    }

    public void RemoveLayer()
    {
        if (listItensContent.Count > 0)
        {
            GameObject lastItem = listItensContent.LastOrDefault();
            listItensContent.Remove(lastItem);
            Destroy(lastItem);
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
