using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Path : MonoBehaviour
{
    public Transform[] waypoints;

    private void Reset()
    {
        waypoints = GetComponentsInChildren<Transform>().Where(x => x != transform).ToArray();
        int aux = 0;
        foreach (var item in waypoints)
        {
            item.name = item.name + aux++;
           
        }
    }
}
