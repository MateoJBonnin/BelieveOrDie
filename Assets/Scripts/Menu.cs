using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button PlayButton;
    public Button CreditsButton;
    public Button OptionButton;
    public Button backCreditsButton;
    public Button backOptionsButton;

    public GameObject credits;
    public GameObject menu;
    public GameObject option;


    private void Start()
    {
        PlayButton.onClick.AddListener(StartGame);
        CreditsButton.onClick.AddListener(ShowCredits);
        OptionButton.onClick.AddListener(ShowOptions);
        backCreditsButton.onClick.AddListener(ShowMenu);
        backOptionsButton.onClick.AddListener(ShowMenu);
    }

    public void StartGame()
    {
        PlayButton.onClick.RemoveAllListeners();
        CreditsButton.onClick.RemoveAllListeners();
        OptionButton.onClick.RemoveAllListeners();
        backCreditsButton.onClick.RemoveAllListeners();
        backOptionsButton.onClick.RemoveAllListeners();

        if (PlayerPrefs.GetInt("tuto",0) == 0)
        {
            LevelManager.instance.ChangeScene(1);
        }
        else
        {
            LevelManager.instance.ChangeScene(PlayerPrefs.GetInt("lvl", 2));
        }
    }

    public void ShowMenu()
    {
        credits.gameObject.SetActive(false);
        option.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
    }

    public void ShowOptions()
    {
        credits.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        option.gameObject.SetActive(true);
    }

    public void ShowCredits()
    {
        option.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        credits.gameObject.SetActive(true);
    }
}
