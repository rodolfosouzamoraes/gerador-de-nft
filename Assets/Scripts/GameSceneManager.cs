using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe responsável por gerenciar o carregamento da cena
/// </summary>
public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Reinicia a cena
    /// </summary>
    public static void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
