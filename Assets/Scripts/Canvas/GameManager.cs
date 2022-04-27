using UnityEngine;

/// <summary>
/// Classe responsável por gerenciar os elementos principais da cena.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GenerateNFT GenerateNFT;
    public static PannelInteractionUserCtlr InteractionUser;
    public static PannelLayersCtlr Layers;
    public static DisplayDialogCtlr DisplayDialog;

    private void Awake()
    {
        if (Instance == null)
        {
            GenerateNFT = GetComponent<GenerateNFT>();
            InteractionUser = GetComponent<PannelInteractionUserCtlr>();
            Layers = GetComponent<PannelLayersCtlr>();
            DisplayDialog = FindObjectOfType<DisplayDialogCtlr>();
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

    /// <summary>
    /// Presseta a exibição do tutorial
    /// </summary>
    public void SetViewTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
        pnlQuestionTutorial.SetActive(false);
    }

    /// <summary>
    /// Exibe o tutorial
    /// </summary>
    public void ViewTutorial()
    {
        SetViewTutorial();
        pnlTutorial.SetActive(true);
    }
}
