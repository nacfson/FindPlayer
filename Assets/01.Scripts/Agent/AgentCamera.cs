using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
public class AgentCamera : MonoBehaviourPunCallbacks{    


    private CinemachineVirtualCamera _followCam;
    private AgentInput _agentInput;
    private PhotonView _PV;
    private ActionData _actionData;

    public Player player;

    private void Awake() {
        _followCam = GetComponentInChildren<CinemachineVirtualCamera>();
        _agentInput = GetComponent<AgentInput>();
        _PV = GetComponent<PhotonView>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
        player = PhotonNetwork.LocalPlayer;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Start() {
        //_agentInput.OnMouseScroll += OnScrollHandle;   
        _agentInput.OnMouseInput += OnMouseHandle; 


        string nickName = PhotonNetwork.LocalPlayer.NickName;
        CameraManager.Instance.AddCamera(this);
    }

    private void OnMouseHandle(float x, float y){
        if(_PV.IsMine == false) {
            return;
        }
    }

    public CinemachineVirtualCamera GetCamera(){
        return _followCam;
    }

    public Player GetPlayer(){
        return player;
    }

}