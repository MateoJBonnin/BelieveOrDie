using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Farm : MonoBehaviour
{
    public Transform[] workingSpaces;
    private void Reset()
    {
        workingSpaces = GetComponentsInChildren<Transform>().Where(x => x != transform).ToArray();
        int aux = 0;
        foreach (var item in workingSpaces)
        {
            item.name = item.name + aux++;
        }
    }
}
