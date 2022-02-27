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

        mainCam.OnMove += OnCameraMove;
    }

    private void OnCameraMove(Vector3 move)
    {
        windSfx.pitch = move.magnitude.Remap(0, 40, 1, 1.5f);
        windSfx.volume = move.magnitude.Remap(0, 40, 0.03f, 0.08f);
        windSfx.volume += (mainCam.ZoomPercent - 1f).Remap(0, 1, 0.05f, 0.09f);
    }
}
