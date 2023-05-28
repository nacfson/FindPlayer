using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AgentHP : MonoBehaviour{
    public UnityEvent OnDead;
    protected AgentAnimator _agentAnimator;

    private void Awake() {
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
    }
    public void Damaged(){
        DeadProcess();
    }

    private void DeadProcess(){
        _agentAnimator.OnDead(true);
        OnDead?.Invoke();
    }
}