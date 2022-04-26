using UnityEngine;
using UnityEngine.UI;

public class DisplayDialogCtlr : MonoBehaviour
{
    public GameObject pnlDisplay;
    public Text txtTitle;
    public Text txtMessage;
    
    public void ShowDialog(string title, string message)
    {
        txtTitle.text = title;
        txtMessage.text = message;
        pnlDisplay.gameObject.SetActive(true);
    }

    public void CloseDialog()
    {
        pnlDisplay.gameObject.SetActive(false);
    }

}
