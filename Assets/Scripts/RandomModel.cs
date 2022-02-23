using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomModel : MonoBehaviour
{
    public MeshFilter mf;
    public List<Mesh> Models;

    private void Start()
    {
        mf = GetComponentInChildren<MeshFilter>();
        mf.mesh = Models[Random.Range(0,Models.Count)];
    }
}
