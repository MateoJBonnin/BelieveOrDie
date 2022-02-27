using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public AudioSource buttonSound;
    public Button PlayButton;
    public Button CreditsButton;
    public Button OptionButton;
    public Button backCreditsButton;
    public Button backOptionsButton;
    public Button exitGameButton;
    public Button exitNo;
    public Button exitYes;

    public GameObject credits;
    public GameObject menu;
    public GameObject option;

    public GameObject exitView;

    public Options options;

    private void Start()
    {
        options.Setup();

        PlayButton.onClick.AddListener(StartGame);
        CreditsButton.onClick.AddListener(ShowCredits);
        OptionButton.onClick.AddListener(ShowOptions);
        backCreditsButton.onClick.AddListener(ShowMenu);
        backOptionsButton.onClick.AddListener(ShowMenu);
        exitNo.onClick.AddListener(ShowMenu);
        exitGameButton.onClick.AddListener(ShowAreYouSure);
        exitYes.onClick.AddListener(() => Application.Quit());
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
        exitView.SetActive(false);
        credits.SetActive(false);
        option.SetActive(false);
        menu.SetActive(true);

        buttonSound.Play();
    }

    public void ShowOptions()
    {
        exitView.SetActive(false);
        credits.SetActive(false);
        menu.SetActive(false);
        option.SetActive(true);

        buttonSound.Play();
    }

    public void ShowCredits()
    {
        exitView.SetActive(false);
        option.SetActive(false);
        menu.SetActive(false);
        credits.SetActive(true);

        buttonSound.Play();
    }

    public void ShowAreYouSure()
    {
        exitView.SetActive(true);
        option.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);

        buttonSound.Play();
    }
}
