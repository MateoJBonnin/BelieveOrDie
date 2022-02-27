using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class GameOverCondition : MonoBehaviour
{
    public FaithManager faithManager;

    [SerializeField]
    private Transform canvasParent;
    [SerializeField]
    private GameObject gameWonPanelPrefab;
    [SerializeField]
    private GameObject gameLostPanelPrefab;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private GodHand godHand;

    private GameObject gameWonPanelInstance;
    private GameObject gameLostPanelInstance;
    public float atheismPercentageToLose = .8f;
    private bool passTutorial;

    private void Awake()
    {
        this.faithManager.OnAtheismChanged += this.OnAtheismChangedHandler;
    }

    private void OnAtheismChangedHandler(float atheismPercentage)
    {
        if (atheismPercentage >= this.atheismPercentageToLose)
        {
            this.faithManager.OnAtheismChanged -= this.OnAtheismChangedHandler;
            this.DeactivateInputs();
            this.StartCoroutine(this.StartLoseEndgameMessages());
        }
        else if (atheismPercentage == 0)
        {
            this.faithManager.OnAtheismChanged -= this.OnAtheismChangedHandler;
            this.DeactivateInputs();
            this.StartCoroutine(this.StartWinEndgameMessages());
        }
    }

    private IEnumerator StartLoseEndgameMessages()
    {
        this.gameLostPanelInstance = Instantiate(this.gameLostPanelPrefab, this.canvasParent);
        TutorialMessage.instance.ShowMessage("You could not successfully prove your existence.");

        yield return new WaitForSeconds(2);
        yield return new WaitUntil(() => this.passTutorial);

        LevelManager.instance.ChangeScene(0);
    }

    private IEnumerator StartWinEndgameMessages()
    {
        this.gameWonPanelInstance = Instantiate(this.gameWonPanelPrefab, this.canvasParent);

        TutorialMessage.instance.ShowMessage("You have proved that you are REAL!");
        yield return new WaitForSeconds(2);
        yield return new WaitUntil(() => this.passTutorial);

        int actualLevel = PlayerPrefs.GetInt("lvl", 2);

        if (actualLevel == 3)
        {
            TutorialMessage.instance.ShowMessage("For good this time, I swear");

            yield return new WaitForSeconds(2);
            yield return new WaitUntil(() => this.passTutorial);

            LevelManager.instance.ChangeScene(0);

            PlayerPrefs.SetInt("Won", 1);
        }
        else
        {
            TutorialMessage.instance.ShowMessage("Just kidding you still have more work to do");

            yield return new WaitForSeconds(2);
            yield return new WaitUntil(() => this.passTutorial);

            TutorialMessage.instance.ShowMessage("Now we move to the next town");

            yield return new WaitForSeconds(2);
            yield return new WaitUntil(() => this.passTutorial);

            LevelManager.instance.ChangeScene(3);
        }

        int asd = PlayerPrefs.GetInt("lvl", 2) == 2 ? 3 : 2;
        PlayerPrefs.SetInt("lvl", asd);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.passTutorial = true;
        }
        else
        {
            this.passTutorial = false;
        }
    }

    private void DeactivateInputs()
    {
        this.cameraController.SetMovementActivate(false);
        this.cameraController.SetDragActivate(false);
        this.cameraController.SetZoomActivate(false);
        this.godHand.InputBlock = true;
    }
}
