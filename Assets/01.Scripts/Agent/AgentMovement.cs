using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AgentMovement : MonoBehaviour{
    [SerializeField]
    protected MovementData _movementData;
    protected Vector3 _movementVelocity;
    protected float _verticalSpeed;
    protected float _gravity = -9.81f;
    protected CharacterController _controller;
    protected AgentInput _agentInput;
    protected AgentAnimator _agentAnimator;
    protected float _rotateSpeed = 8f;
    private void Awake() {
        _controller = GetComponent<CharacterController>();
        _agentInput = GetComponent<AgentInput>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
    }
    private void Start() {
        _agentInput.OnMovementKeyPress += SetMovementVelocity;
        _agentInput.OnJumpKeyPress += Jump;
    }
    private void FixedUpdate(){
        if(_movementVelocity.magnitude > 0f){
            Quaternion rotation = Quaternion.LookRotation(_movementVelocity,Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation,rotation,_rotateSpeed * Time.deltaTime);

            _agentAnimator.SetSpeed(1f);
        }
        else{
            _agentAnimator.SetSpeed(0f);
        }
        if(_controller.isGrounded == false){
            _verticalSpeed += _gravity * Time.fixedDeltaTime;
        }
        else if(_controller.isGrounded == true){
            _agentAnimator.OnJump(false);
        }
        Vector3 move = (_movementVelocity * _movementData.Speed + _verticalSpeed * Vector3.up) * Time.fixedDeltaTime;
        _controller.Move(move);
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