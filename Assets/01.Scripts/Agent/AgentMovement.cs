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
    protected PlayerSound _playerSound;

    private Transform _cameraTransform;
    protected ActionData _actionData;

    public PhotonView PV;
    [SerializeField]
    protected float _rotateSpeed = 30f;
    protected virtual void Awake() {
        _controller = GetComponent<CharacterController>();
        _agentInput = GetComponent<AgentInput>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
        _playerSound = GetComponent<PlayerSound>();
        _currentSpeed = _movementData.Speed;
        PV = GetComponent<PhotonView>();
    }
    protected virtual void Start() {
        _agentInput.OnMovementKeyPress += SetMovementVelocity;
        _agentInput.OnJumpKeyPress += Jump;
        _cameraTransform = Define.MainCam.transform;
        if(PV.IsMine == false) {
            //GetComponentInChildren<Camera>().gameObject.SetActive(false);
            GetComponentInChildren<CinemachineVirtualCamera>().enabled = false;
        }
    }
    //속도 자연스럽게
    protected virtual void FixedUpdate(){
        if(PV.IsMine == false) {
            return;
        }
        if (_actionData.IsAttacking || _actionData.IsPenalty) {
            return;
        }
        
        _actionData.IsGrounded = _controller.isGrounded;

        GAME_STATE currentState = RoomManager.Instance.CurrentState;
        if(currentState == GAME_STATE.LOADING  || currentState == GAME_STATE.UI || currentState == GAME_STATE.CHAT) return;
        if(Input.GetKey(KeyCode.LeftShift)){
            SetRunSpeed(true);
        }
        else{
            SetRunSpeed(false);
        }

        _agentAnimator.SetSpeed(_movementVelocity.sqrMagnitude);

        if(_controller.isGrounded == false){
            _verticalSpeed += _gravity * Time.fixedDeltaTime * 0.1f;
        }
        else if(_controller.isGrounded == true){
            _agentAnimator.OnJump(false);
            _agentAnimator.IsJump(false);
        }

        Vector3 cameraForward = Vector3.Scale(_cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraRight = Vector3.Scale(_cameraTransform.right, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = (_movementVelocity.x * cameraRight + _movementVelocity.z * cameraForward) * _currentSpeed * Time.fixedDeltaTime + Vector3.up * _verticalSpeed;

        if(_movementVelocity.magnitude > 0f){
            SetLerpRotation(movement + transform.position,_rotateSpeed);
        }
        else if(_movementVelocity.sqrMagnitude < 0.1f){
            _agentAnimator.SetBoolRun(false);
        }
        _controller.Move(movement);
    }


    public void SetRunSpeed(bool result) {
        if(result){
            _currentSpeed = _movementData.RunSpeed;
        }
        else {
            _currentSpeed = _movementData.Speed;
        }
        _actionData.IsRunning = result;
        _agentAnimator.SetBoolRun(result);
    }
    public void SetLerpRotation(Vector3 target, float speed){
        _actionData.IsRotating  = true;
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        // 현재의 회전과 목표 회전 사이를 선형 보간하여 시간을 다르게 해줍니다.
        float lerpSpeed = _rotateSpeed * Time.deltaTime * 0.3f;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed);

        if (transform.rotation == targetRotation){
            _actionData.IsRotating = false;
        }
    }

    public void SetMovementVelocity(Vector3 velocity){
        _movementVelocity = velocity;
    }

    public void Jump(){
        GAME_STATE currentState = RoomManager.Instance.CurrentState;
        if(currentState == GAME_STATE.LOADING || currentState == GAME_STATE.UI || currentState == GAME_STATE.CHAT) return;
        
        if(_controller.isGrounded){
            _agentAnimator.OnJump(true);
            _agentAnimator.IsJump(true);
            _verticalSpeed = _movementData.JumpHeight * 0.2f;
        }
    }
}