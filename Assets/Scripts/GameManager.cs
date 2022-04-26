using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GenerateNFT pnlGenerateNFT;
    public static PannelInteractionUserCtlr pnlInteractionUser;
    public static PannelLayersCtlr pnlLayers;
    public static DisplayDialogCtlr pnlDisplayDialog;

    private void Awake()
    {
        if (Instance == null)
        {
            pnlGenerateNFT = GetComponent<GenerateNFT>();
            pnlInteractionUser = GetComponent<PannelInteractionUserCtlr>();
            pnlLayers = GetComponent<PannelLayersCtlr>();
            pnlDisplayDialog = FindObjectOfType<DisplayDialogCtlr>();
            Instance = this;
            return;
        }
        Destroy(this);
    }

    public GameObject pnlQuestionTutorial;
    public GameObject pnlTutorial;

    void Start()
    {
        if (PlayerPrefs.GetInt("Tutorial") == 0)
        {
            pnlQuestionTutorial.SetActive(true);
        }
    }

    public void SetViewTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        pnlQuestionTutorial.SetActive(false);
    }

    public void ViewTutorial()
    {
        SetViewTutorial();
        pnlQuestionTutorial.SetActive(true);
    }
}
