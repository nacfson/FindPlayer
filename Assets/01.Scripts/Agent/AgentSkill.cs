using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSkill : MonoBehaviour{
    protected AgentInput _agentInput;
    protected AgentAnimator _agentAnimator;
    protected ActionData _actionData;

    private void Awake() {
        _agentInput = GetComponent<AgentInput>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
    }

    private void Start() {
        _agentInput.OnAttackKeyPress += StartAttack;
        _agentAnimator.OnAttackTrigger += Attack;
    }
    private void Attack(){
        bool result = Physics.SphereCast(transform.position - transform.forward * 0.5f , 0.5f,transform.forward,out RaycastHit hit, 1f,1 << LayerMask.NameToLayer("PLAYER"));
        Debug.Log($"Result: {result}");

        if(result){
            if(hit.collider.TryGetComponent<AgentHP>(out AgentHP agentHP)){
                agentHP.Damaged();
            }
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - transform.forward * 0.5f ,0.5f);
    }

    private void StartAttack(){
        if(_actionData.IsAttacking == false){
            _actionData.IsAttacking = true;
            _agentAnimator.SetBoolAttack(true);
            _agentAnimator.SetTriggerAttack();
        }
    }
}