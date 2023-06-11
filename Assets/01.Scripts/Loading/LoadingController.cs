using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingController : AgentController{
    protected override void Awake() {
        _actionData = transform.Find("AD").GetComponent<ActionData>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _agentHP = transform.GetComponent<AgentHP>();
    }

    protected override void GetMouseClickInput() {
        if (_actionData.IsDead) {
            if (Input.GetMouseButtonDown(0)) {
                _cameraIndex = (_cameraIndex + 1) % CameraManager.Instance.GetCameraCount();
                CameraManager.Instance.ChangeCamera(_cameraIndex);
            }
        }
    }
}