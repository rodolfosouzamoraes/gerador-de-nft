using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDialogCtlr : MonoBehaviour
{
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtMessage;
    
    public void ShowDialog(string title, string message)
    {
        txtTitle.text = title;
        txtMessage.text = message;
        this.gameObject.SetActive(true);
    }

    public void CloseDialog()
    {
        this.gameObject.SetActive(false);
    }

}
