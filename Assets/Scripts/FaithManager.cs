using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
#endif
public class FaithManager : MonoBehaviour
{
    public List<Villager> faiths;

    public Slider slider;

    public int totalPob;
    public int atheist;

    public float atheismPercentage;
    public Action<float, int> OnAtheismChanged;

    public TMPro.TextMeshProUGUI atheistCountText;

    public GameOverCondition gameOverCondition;
    public void Start()
    {
        if (gameOverCondition != null)
        {
            totalPob = faiths.Count;
            foreach (Villager villager in faiths)
            {
                villager.OnDie += OnDieHandler;
                villager.faithController.OnConvertedToAtheist += OnConvertedToAtheistHandler;
            }
        }
    }

    private void OnConvertedToAtheistHandler()
    {
        atheist++;
        atheistCountText.text = aliveAtheistCount.ToString();
        CheckFaith();
    }

    private void OnDieHandler(Villager v)
    {
        if(!v.IsAtheist)
            atheist++;
        CheckFaith();
        FaithTextFeedback.Instance.CreateFeedbak(v.IsAtheist, Camera.main.WorldToScreenPoint(v.transform.position + Vector3.up * 2f));
    }

    public int aliveAtheistCount;
    private void CheckFaith()
    {
        aliveAtheistCount = this.faiths.Count(x => x != null && !x.isDead && x.IsAtheist);
        atheismPercentage = atheist / (float) totalPob;
        OnAtheismChanged?.Invoke(atheismPercentage, aliveAtheistCount);
        slider.value = 1 - gameOverCondition.losePercentage;
        atheistCountText.text = aliveAtheistCount.ToString();
    }

    private void OnValidate()
    {
        faiths = FindObjectsOfType<Villager>().Where(a => a.rol == Roles.Villager).ToList();
    }
    
    
        
#if UNITY_EDITOR
    static FaithManager()
    {
        UnityEditor.SceneManagement.EditorSceneManager.sceneSaving += OnSaving;
    }

    private static void OnSaving(Scene scene, string path)
    {
        FindObjectOfType<FaithManager>().faiths = FindObjectsOfType<Villager>().Where(a => a.rol == Roles.Villager).ToList();
        Debug.Log("OnSaving FaithManager");
    }
#endif
}
