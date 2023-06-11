using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSkill : AgentSkill{
    protected override void Awake() {
        _agentInput = GetComponent<AgentInput>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
        _agentMovement = GetComponent<AgentMovement>();
    }
    protected override void StartAttack(){
        if(_actionData.IsAttacking == false){
            if (_targetCol != null) {
                transform.rotation = Quaternion.LookRotation(_targetCol.transform.root.position - transform.position);
            }
            
            _actionData.IsAttacking = true;
            _agentAnimator.SetBoolAttack(true);
            _agentAnimator.SetTriggerAttack();
            _agentMovement.SetRunSpeed(false);
        }
    }
}