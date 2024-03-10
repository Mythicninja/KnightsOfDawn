using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cvc;
    public void SetPlayerCameraFollow() {
        cvc = FindObjectOfType<CinemachineVirtualCamera>();
        cvc.Follow = PlayerController.Instance.transform;
    }
}
