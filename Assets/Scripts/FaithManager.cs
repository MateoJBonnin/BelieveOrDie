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
    public Action<float> OnAtheismChanged;
    public void Start()
    {
        totalPob = faiths.Count;
        foreach (Villager villager in faiths)
        {
            villager.OnDie += OnDieHandler;
            villager.faithController.OnConvertedToAtheist += OnConvertedToAtheistHandler;
        }
    }

    private void OnConvertedToAtheistHandler()
    {
        atheist++;
        CheckFaith();
    }

    private void OnDieHandler(Villager v)
    {
        if (v.IsAtheist)
            atheist--;
        totalPob--;
        CheckFaith();
        FaithTextFeedback.Instance.CreateFeedbak(v.IsAtheist, Camera.main.WorldToScreenPoint(v.transform.position + Vector3.up * 2f));
    }

    private void CheckFaith()
    {
        atheismPercentage = atheist / (float) totalPob;
        OnAtheismChanged?.Invoke(atheismPercentage);
        slider.value = (1 - atheismPercentage);
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
