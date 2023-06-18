using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class AgentController : MonoBehaviour {
    protected PhotonView _PV;
    protected ActionData _actionData;
    protected AgentAnimator _agentAnimator;
    protected AgentHP _agentHP;
    protected int _cameraIndex = 0;
    
    protected virtual void Awake() {
        _PV = GetComponent<PhotonView>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _agentHP = transform.GetComponent<AgentHP>();
    }
    private void OnEnable() {
        RoomManager.Instance.OnRoundEnd += DancePlayer;
    }
    private void OnDisable() {
        RoomManager.Instance.OnRoundEnd -= DancePlayer;
    }
    public void DancePlayer(Player player) {
        bool result = (PhotonNetwork.LocalPlayer == player);
        if (result) {
            _agentAnimator.DoDance();
        }
    }

    protected void Update() {
        GetMouseClickInput();
    }

    protected virtual void GetMouseClickInput() {
        if (_PV.IsMine == false) return;
        if (_actionData.IsDead) {
            if (Input.GetMouseButtonDown(0)) {
                _cameraIndex = (_cameraIndex + 1) % CameraManager.Instance.GetCameraCount();
                CameraManager.Instance.ChangeCamera(_cameraIndex);
            }
        }
    }

    public void MethodRpc() {
        _PV.RPC("OnDeadRpc", RpcTarget.All, true);
    }
    
    [PunRPC]
    public void OnDeadRpc(bool result) {
        if (_PV.IsMine) {
            _agentAnimator.OnDead(result);
            _agentAnimator.IsDead(result);
            _agentHP.OnDead?.Invoke();
            _actionData.IsDead = true;
            CameraManager.Instance.ChangeCamera(_cameraIndex);
        }
    }
}