using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]

public class AgentMovement : MonoBehaviourPun{
    [SerializeField]
    protected MovementData _movementData;
    protected Vector3 _movementVelocity;
    protected float _verticalSpeed;
    protected float _gravity = -9.81f;
    protected CharacterController _controller;
    protected AgentInput _agentInput;
    protected AgentAnimator _agentAnimator;
    protected float _currentSpeed;
    private Transform _cameraTransform;
    protected ActionData _acionData;

    public PhotonView PV;
    [SerializeField]
    protected float _rotateSpeed = 30f;
    private void Awake() {
        _controller = GetComponent<CharacterController>();
        _agentInput = GetComponent<AgentInput>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _acionData = transform.Find("AD").GetComponent<ActionData>();
        _currentSpeed = _movementData.Speed;
        PV = GetComponent<PhotonView>();
    }
    private void Start() {
        _agentInput.OnMovementKeyPress += SetMovementVelocity;
        _agentInput.OnJumpKeyPress += Jump;
        _cameraTransform = Define.MainCam.transform;
        if(PV.IsMine == false) {
            //GetComponentInChildren<Camera>().gameObject.SetActive(false);
            GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;
        }
    }
    //속도 자연스럽게
    private void FixedUpdate(){
        if(PV.IsMine == false) {
            return;
        }
        if (_acionData.IsAttacking) {
            return;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            SetRunSpeed(true);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift)){
            SetRunSpeed(false);
        }

        _agentAnimator.SetSpeed(_movementVelocity.sqrMagnitude);

        if(_controller.isGrounded == false){
            _verticalSpeed += _gravity * Time.fixedDeltaTime;
        }
        else if(_controller.isGrounded == true){
            _agentAnimator.OnJump(false);
        }
        
        Vector3 cameraForward = Vector3.Scale(_cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraRight = Vector3.Scale(_cameraTransform.right, new Vector3(1, 0, 1)).normalized;
        Vector3 move = (_movementVelocity.x * cameraRight + _movementVelocity.z * cameraForward) * _currentSpeed * Time.fixedDeltaTime + Vector3.up * _verticalSpeed;

        if(_movementVelocity.magnitude > 0f){
            SetLerpRotation(move + transform.position,_rotateSpeed);
        }
        else if(_movementVelocity.sqrMagnitude < 0.1f){
            _agentAnimator.SetBoolRun(false);
        }
        _controller.Move(move);

    }

    public void SetRunSpeed(bool result) {
        if(result){
            _currentSpeed = _movementData.RunSpeed;
        }
        else {
            _currentSpeed = _movementData.Speed;
        }
        _acionData.IsRunning = result;
        _agentAnimator.SetBoolRun(result);
    }
    public void SetLerpRotation(Vector3 target, float speed){
        //_actionData.isRotate = true;
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation,rotation, speed * Time.deltaTime);

        if(transform.rotation == rotation){
            //_actionData.isRotate = false;
        }
    }

    public void SetMovementVelocity(Vector3 velocity){
        _movementVelocity = velocity;
    }

    public void Jump(){
        if(_controller.isGrounded){
            _agentAnimator.OnJump(true);
            _verticalSpeed = _movementData.JumpHeight;
        }
    }
}