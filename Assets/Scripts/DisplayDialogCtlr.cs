using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe responsável por exibir o dialogo personalizado ao usuário
/// </summary>
public class DisplayDialogCtlr : MonoBehaviour
{
    public GameObject pnlDisplay;
    public Text txtTitle;
    public Text txtMessage;

    /// <summary>
    /// Exibe painel do dialogo
    /// </summary>/
    /// <param name="title">Titulo do painel</param>
    /// <param name="message">Menssagem do painel</param>
    public void ShowDialog(string title, string message)
    {
        txtTitle.text = title;
        txtMessage.text = message;
        pnlDisplay.gameObject.SetActive(true);
    }

    /// <summary>
    /// Fecha o painel
    /// </summary>
    public void CloseDialog()
    {
        pnlDisplay.gameObject.SetActive(false);
    }

}
