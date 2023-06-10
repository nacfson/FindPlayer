using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
public class AgentAnimator : MonoBehaviourPun{
    protected readonly int _speedHash = Animator.StringToHash("SPEED");
    protected readonly int _jumpHash = Animator.StringToHash("JUMP");
    protected readonly int _attackHash = Animator.StringToHash("ATTACK");
    protected readonly int _attackBoolHash = Animator.StringToHash("IS_ATTACK");
    protected readonly int _runBoolHash = Animator.StringToHash("IS_RUN");
    protected readonly int _deadHash = Animator.StringToHash("DEAD");
    protected readonly int _deadBoolHash = Animator.StringToHash("IS_DEAD");
    protected readonly int _jumpBoolHash = Animator.StringToHash("IS_JUMP");
    protected Animator _animator;
    protected ActionData _actionData;

    public PhotonView PV;

    public event Action OnAttackEndTrigger;
    public event Action OnAttackTrigger;
    public event Action OnWalkTrigger;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _actionData = transform.parent.Find("AD").GetComponent<ActionData>();
        PV = transform.parent.GetComponent<PhotonView>();
    }
    public void SetSpeed(float speed){
        _animator.SetFloat(_speedHash,speed);
    }
    public void SetBoolRun(bool result){
        _animator.SetBool(_runBoolHash,result);
    }

    public void OnJump(bool result){
        if(result){
            _animator.SetTrigger(_jumpHash);
        }
        else{
            _animator.ResetTrigger(_jumpHash);
        }
    }
    public void OnDead(bool result){
        if(result){
            _animator.SetTrigger(_deadHash);
        }
        else{
            _animator.ResetTrigger(_deadHash);
        }
    }
    public void IsJump(bool result){
        _animator.SetBool(_jumpBoolHash,result);
    }
    public void IsDead(bool result) {
        _animator.SetBool(_deadBoolHash, result);
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
        OnAttackEndTrigger?.Invoke();
    }
    public void OnWalkAnimation() {
        OnWalkTrigger?.Invoke();
        Debug.Log("OnWalkAnimation");
    }
    public void OnAttack(){
        OnAttackTrigger?.Invoke();
    }
}