using UnityEngine;

/// <summary>
/// Classe respons�vel por possibilitar ao Animator acessar o m�todo e fechar a tela de tutorial automaticamente.
/// </summary>
public class PannelTutorialCtlr : MonoBehaviour
{
    public GameObject pnlTutorial;

    /// <summary>
    /// Fecha a tela de tutorial
    /// </summary>
    public void CloseTutorial() // O animator acessa essa fun��o para poder fechar o tutorial automaticamente
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        pnlTutorial.SetActive(false);
    }
}
