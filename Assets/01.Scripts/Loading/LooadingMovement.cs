using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooadingMovement : AgentMovement{
    protected override void Awake(){
        _controller = GetComponent<CharacterController>();
        _agentInput = GetComponent<AgentInput>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
        _playerSound = GetComponent<PlayerSound>();
        _currentSpeed = _movementData.Speed;
    }

    
    protected override void Start() {
        _agentInput.OnMovementKeyPress += SetMovementVelocity;
        _agentInput.OnJumpKeyPress += Jump;
    }
    protected override void FixedUpdate(){
        if (_actionData.IsAttacking) {
            return;
        }
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

        Vector3 movement = _movementVelocity * _currentSpeed * Time.fixedDeltaTime + Vector3.up * _verticalSpeed;

        if(_movementVelocity.magnitude > 0f){
            SetLerpRotation(movement + transform.position,_rotateSpeed);
        }
        else if(_movementVelocity.sqrMagnitude < 0.1f){
            _agentAnimator.SetBoolRun(false);
        }
        _controller.Move(movement);
    }
}