using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public string link;


    private void OnMouseDown()
    {
        Open();
    }
    public void Open()
    {
        Application.OpenURL(link);
    }
}
