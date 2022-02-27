using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSfxController : MonoBehaviour
{
    private CameraController mainCam;
    private AudioSource windSfx;

    void Start()
    {
        mainCam = Camera.main.GetComponent<CameraController>();
        windSfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log( mainCam.ZoomPercent - 1);
        windSfx.volume = mainCam.ZoomPercent - 1f;
    }
}
