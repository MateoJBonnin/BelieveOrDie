using System;
using UnityEngine;

public class GameOverCondition : MonoBehaviour
{
    public FaithManager faithManager;

    public float atheismPercentageToLose = .8f;
    
    private void Awake()
    {
        faithManager.OnAtheismChanged += OnAtheismChangedHandler;
    }

    private void OnAtheismChangedHandler(float atheismPercentage)
    {

        if (atheismPercentage >= atheismPercentageToLose)
        {
            faithManager.OnAtheismChanged -= OnAtheismChangedHandler;
            Debug.LogError("You Lose!");
        }
        else if (atheismPercentage == 0)
        {
            faithManager.OnAtheismChanged -= OnAtheismChangedHandler;
            Debug.LogError("You Win!");
        }
    }
}
