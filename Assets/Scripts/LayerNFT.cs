using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe responsável por ter as sprites das camadas.
/// </summary>
public class LayerNFT
{
    public List<Sprite> listLayer { get; set; }

    public LayerNFT()
    {
        this.listLayer = new List<Sprite>();
    }
}
