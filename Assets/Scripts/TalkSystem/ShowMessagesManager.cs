using System;
using UnityEngine;

public class ShowMessagesManager : MonoBehaviour
{
    public static ShowMessagesManager instance;
    public Transform god;
    public bool show;
    public float showMessageHeight = 10;

    public Action<bool> OnShowChanged;
    
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        bool newShow = god.position.y < showMessageHeight;
        if (newShow != show)
        {
            show = newShow;
            OnShowChanged?.Invoke(show);
        }
    }
}
