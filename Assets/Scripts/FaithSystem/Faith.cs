using System;
using UnityEngine;

public class Faith : MonoBehaviour
{
    public float value;

    public event Action OnConverted;
    
    public void Add(float amount)
    {
        value = Mathf.Clamp(value + amount, -1, 1);
        if (value < 0)
        {
            OnConverted?.Invoke();
        }        
    }
}
