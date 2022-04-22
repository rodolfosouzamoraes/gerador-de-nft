using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    public static int countNFTs = 0;

    public static void IncrementCountNFT()
    {
        countNFTs++;
        if (countNFTs>50)
        {
            countNFTs = 0;
            SceneManager.LoadScene(0);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
