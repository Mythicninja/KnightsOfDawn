using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    
    private void Start() {
        if (transitionName == SceneManage.Instance.SceneTransitionName) {
            PlayerController.Instance.transform.position = transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            
            UIFade.Instance.FadeToClear();
        }
    }
}
