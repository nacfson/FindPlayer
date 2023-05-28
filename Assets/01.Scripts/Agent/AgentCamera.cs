using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class AgentCamera : MonoBehaviour{    

    public Vector3 finarDir;    
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _maxOrthoSize = 15f;

    [SerializeField] private float _followSpeed = 10f;
    [SerializeField] private float _sensitivity = 30f;
    [SerializeField] private float _clampAngle = 70f;

    private float _rotX;
    private float _rotY;

    public Transform realCamera;
    public Vector3 dirNormal;
    public float maxDistance;
    public float minDistance;
    public float finalDistance;
    
    private CinemachineVirtualCamera _followCam;
    private AgentInput _agentInput;

    private void Awake() {
        _followCam = FindObjectOfType<CinemachineVirtualCamera>();
        _agentInput = GetComponent<AgentInput>();
    }
    
    private void Start() {
        //_agentInput.OnMouseScroll += OnScrollHandle;   
        _agentInput.OnMouseInput += OnMouseHandle; 

        _rotX = _followCam.transform.localRotation.eulerAngles.x;
        _rotY = _followCam.transform.localRotation.eulerAngles.y;
    }

    private void OnScrollHandle(float value){
        _followCam.m_Lens.FieldOfView -= value * _zoomSpeed;
        _followCam.m_Lens.FieldOfView = Mathf.Clamp(_followCam.m_Lens.FieldOfView,3f,_maxOrthoSize);
    }

    private void OnMouseHandle(float x, float y){
        _rotX += -y * _sensitivity * Time.deltaTime;
        _rotY += x * _sensitivity * Time.deltaTime;

        _rotX = Mathf.Clamp(_rotX,0f,_clampAngle);
        Quaternion rot = Quaternion.Euler(_rotX,_rotY,0);
        _followCam.transform.rotation = rot;
    }

}