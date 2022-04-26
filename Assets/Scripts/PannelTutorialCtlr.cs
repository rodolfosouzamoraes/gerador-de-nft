using UnityEngine;

/// <summary>
/// Classe responsável por possibilitar ao Animator acessar o método e fechar a tela de tutorial automaticamente.
/// </summary>
public class PannelTutorialCtlr : MonoBehaviour
{
    public GameObject pnlTutorial;

    /// <summary>
    /// Fecha a tela de tutorial
    /// </summary>
    public void CloseTutorial() // O animator acessa essa função para poder fechar o tutorial automaticamente
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        pnlTutorial.SetActive(false);
    }
}
