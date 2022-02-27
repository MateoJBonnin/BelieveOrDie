using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaithFeedbak : MonoBehaviour
{
    CanvasGroup canvasGroup;
    float verticalSpeed;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        verticalSpeed = Screen.height * 0.1f;
    }

    void Update()
    {
        transform.position += Vector3.up * verticalSpeed * Time.deltaTime;
        canvasGroup.alpha -= Time.deltaTime * 1.5f;
        if (canvasGroup.alpha <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
