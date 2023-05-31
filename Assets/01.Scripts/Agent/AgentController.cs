using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class AgentController : MonoBehaviour {
    protected PhotonView _PV;
    protected ActionData _actionData;

    private void Awake() {
        _PV = GetComponent<PhotonView>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
    }
    private void Update() {
        GetMouseClickInput();
    }
    private void GetMouseClickInput() {
        if (RoomManager.Instance == null) return;
        if (_PV.IsMine == false) return;
        if (_actionData.IsDead) {
            if (Input.GetMouseButtonDown(0)) {
                //OnMouseClicked?.Invoke();
                Debug.Log("ChangeCameraInAgentInput");
                RoomManager.Instance.ChangeCamera();
            }
        }
        else {
            Debug.LogError("It is not dead");
        }

    }
}