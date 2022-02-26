using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button PlayButton;
    public Button CreditsButton;
    public Button backButton;

    public GameObject credits;
    public GameObject menu;


    private void Start()
    {
        PlayButton.onClick.AddListener(StartGame);
        CreditsButton.onClick.AddListener(ShowCredits);
        backButton.onClick.AddListener(ShowCredits);
    }

    public void StartGame()
    {
        PlayButton.onClick.RemoveAllListeners();
        CreditsButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        if (PlayerPrefs.GetInt("tuto",0) == 0)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    public void ShowCredits()
    {
        credits.gameObject.SetActive(!credits.activeInHierarchy);
        menu.gameObject.SetActive(!menu.activeInHierarchy);
    }
}
