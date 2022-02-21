using UnityEngine;

public class Character : MonoBehaviour
{
    public Faith faith;
    public GameObject collider;   
    public SpreadFaith spreadFaith;

    [Header("Faiths Data")]
    public FaithData catholicData;
    public FaithData atheistData;

    [Header("DEBUG Renderers")]
    public Renderer characterRenderer;
    public Renderer spreadRenderer;
    //Remove this Sometime.
    public bool atheist;
    
    public void Awake()
    {
        faith.OnConverted += ConvertToAtheist;
        
        //Remove this Sometime.
        if (atheist)
        {
            faith.Add(-100);
        }
    }

    private void ConvertToAtheist()
    {
        faith.OnConverted -= ConvertToAtheist;
        Convert(atheistData);
    }
    
    private void ConvertToCatholic()
    {
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
