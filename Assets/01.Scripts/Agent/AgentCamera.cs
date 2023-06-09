using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
public class AgentCamera : MonoBehaviourPunCallbacks{    

    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _maxOrthoSize = 15f;

    [SerializeField] private float _followSpeed = 10f;
    [SerializeField] private float _sensitivity = 30f;
    [SerializeField] private float _clampAngle = 70f;

    private float _rotX;
    private float _rotY;
    private CinemachineVirtualCamera _followCam;
    private AgentInput _agentInput;
    private PhotonView _PV;
    private ActionData _actionData;
    public Player player;
    public LayerMask obstacleLayer; // 벽과 같은 장애물을 나타내는 레이어
    private bool _canRotate = true;

    private float _cameraDistance;
    private GAME_STATE _currentState;

    private void Awake() {
        _followCam = GetComponentInChildren<CinemachineVirtualCamera>();
        _agentInput = GetComponent<AgentInput>();
        _PV = GetComponent<PhotonView>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
        player = PhotonNetwork.LocalPlayer;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _cameraDistance = Vector3.Distance(transform.position, _followCam.transform.position);
    }
    
    private void Start() {
        _canRotate = true;
        //_agentInput.OnMouseScroll += OnScrollHandle;   
        _agentInput.OnMouseInput += OnMouseHandle; 

        string nickName = PhotonNetwork.LocalPlayer.NickName;
        CameraManager.Instance.AddCamera(this);


        _rotX = _followCam.transform.localRotation.eulerAngles.x;
        _rotY = _followCam.transform.localRotation.eulerAngles.y;
    }
    private void Update() {
        _currentState = RoomManager.Instance.CurrentState;
    }

    private void LateUpdate(){
        if (_currentState == GAME_STATE.UI || _currentState == GAME_STATE.LOADING) return;
        bool result = Physics.Raycast(_followCam.transform.position, _followCam.transform.forward,out RaycastHit hit,_cameraDistance,obstacleLayer);

        // if(result){
        //     _followCam.Follow = null;
        //     _followCam.transform.position = hit.point + _followCam.transform.forward * 0.5f;
        //     _canRotate = false;
        // }
        // else{
        //     if(_followCam.Follow == null){
        //         _followCam.Follow = this.transform;
        //         _canRotate = true;
        //     }
        // }
    }
    private void OnMouseHandle(float x, float y){
        if(_PV.IsMine == false){
            return;
        }

        if (_currentState == GAME_STATE.UI || _currentState == GAME_STATE.LOADING) return;
        _rotX += -y * _sensitivity * Time.deltaTime;
        _rotY += x * _sensitivity * Time.deltaTime;

        _rotX = Mathf.Clamp(_rotX,0f,_clampAngle);
        Quaternion rot = Quaternion.Euler(_rotX,_rotY,0);
        _followCam.transform.rotation = rot;
    }
    public CinemachineVirtualCamera GetCamera(){
        return _followCam;
    }

    public Player GetPlayer(){
        return player;
    }

}