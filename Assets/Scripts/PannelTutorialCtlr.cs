using UnityEngine;

public class PannelTutorialCtlr : MonoBehaviour
{
    public GameObject pnlTutorial;

    // O animator acessa essa fun��o para poder fechar o tutorial automaticamente
    public void CloseTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        pnlTutorial.SetActive(false);
    }
}
