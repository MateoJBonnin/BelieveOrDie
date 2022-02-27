using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour
{
    public Button mainMenu;
    public Button closeButton;
    public GameObject view;

    private bool paused;
    private GodHand godHand;
    
    private void Start()
    {
        godHand = FindObjectOfType<GodHand>();
        mainMenu.onClick.AddListener(GoToMainMenu);
        closeButton.onClick.AddListener(Close);
    }
     
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;

            if (paused)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    private void Open()
    {
        godHand.InputBlock = true;
        view.transform.DOScale(Vector3.one, .25f);
    }

    public void Close()
    {
        paused = false;
        godHand.InputBlock = false;
        view.transform.DOScale(Vector3.zero, .25f);
    }

    private void GoToMainMenu()
    {
        LevelManager.instance.ChangeScene(0);
    }
}
