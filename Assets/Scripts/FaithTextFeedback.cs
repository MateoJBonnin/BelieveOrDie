using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaithTextFeedback : MonoBehaviour
{
    [SerializeField] GameObject FaithAdd;
    [SerializeField] GameObject FaithSub;

    public static FaithTextFeedback Instance;

    void Awake()
    {
        Instance = this;
    }

    public void CreateFeedbak(bool positive, Vector3 pos)
    {
        Instantiate(positive ? FaithAdd : FaithSub, pos, Quaternion.identity, transform);
    }
}
