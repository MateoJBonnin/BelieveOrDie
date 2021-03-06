using System;
using UnityEngine;

public class FaithController : MonoBehaviour
{
    public Villager villager;
    public Faith faith;
    public GameObject collider;   
    public SpreadFaith spreadFaith;

    [Header("Faiths Data")]
    public FaithData catholicData;
    public FaithData atheistData;

    [Header("DEBUG Renderers")]
    public Renderer characterRenderer;
    public Renderer spreadRenderer;


    public Action OnConvertedToAtheist;
    
    public void Awake()
    {
        faith.OnConverted += ConvertToAtheist;
        SpreadActive(false);
    }

    public void ConvertToAtheist()
    {
        villager.rol = Roles.Atheist;
        faith.OnConverted -= ConvertToAtheist;
        Convert(atheistData);
        OnConvertedToAtheist?.Invoke();
    }
    
    private void ConvertToCatholic()
    {
        villager.rol = Roles.Villager;
        Convert(catholicData);
    }

    private void Convert(FaithData faithData)
    {
        
        characterRenderer.material = faithData.characterMaterial;
        spreadRenderer.material = faithData.spreadMaterial;
        spreadFaith.faithPerSecond = faithData.spreadValue;
        spreadFaith.gameObject.layer =  (int) Mathf.Log(faithData.spreadLayer, 2);
        collider.layer = (int) Mathf.Log(faithData.faithLayer, 2);;
    }

    public void SpreadActive(bool active)
    {
        spreadFaith.enabled = active;
    }
}

[System.Serializable]
public class FaithData
{
    public Material characterMaterial;
    public Material spreadMaterial;
    public float spreadValue;
    public LayerMask faithLayer;
    public LayerMask spreadLayer;
}
