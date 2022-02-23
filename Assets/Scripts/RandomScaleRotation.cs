using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScaleRotation : MonoBehaviour
{
    public float scaleDif;
    public float rotationDif;

    void Start()
    {
        transform.localScale = transform.localScale + Vector3.one * Random.Range(0, scaleDif);
        transform.rotation *= Quaternion.Euler(0, Random.Range(0, rotationDif), 0);
    }

}
