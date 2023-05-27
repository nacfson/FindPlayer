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
        _agentInput.OnAttackKeyPress += Attack;
    }

    private void Attack(){
        _actionData.IsAttacking = true;
        _agentAnimator.SetBoolAttack(true);
        _agentAnimator.SetTriggerAttack();
    }
}