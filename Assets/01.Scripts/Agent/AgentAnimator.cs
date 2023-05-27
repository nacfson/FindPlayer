using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AgentAnimator : MonoBehaviour{
    protected readonly int _speedHash = Animator.StringToHash("SPEED");
    protected readonly int _jumpHash = Animator.StringToHash("JUMP");
    protected readonly int _attackHash = Animator.StringToHash("ATTACK");
    protected readonly int _attackBoolHash =Animator .StringToHash("IS_ATTACK");
    protected Animator _animator;
    protected ActionData _actionData;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _actionData = transform.parent.Find("AD").GetComponent<ActionData>();
    }
    public void SetSpeed(float speed){
        _animator.SetFloat(_speedHash,speed);
    }

    public void OnJump(bool result){
        if(result){
            _animator.SetTrigger(_jumpHash);
        }
        else{
            _animator.ResetTrigger(_jumpHash);
        }
    }

    public void SetTriggerAttack(){
        _animator.SetTrigger(_attackHash);
    }
    public void SetBoolAttack(bool result){
        _animator.SetBool(_attackBoolHash,result);
    }
    public void OnAttackEnd(){
        _animator.ResetTrigger(_attackHash);
        SetBoolAttack(false);
        _actionData.IsAttacking = false;
    }
}