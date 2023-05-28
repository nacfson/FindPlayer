using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour{
    protected List<AITransition> _transitions;
    protected AIBrain _brain;
    protected EnemyController _controller;
    protected ActionData _actionData;
    protected AgentAnimator _agentAnimator;
    protected EnemyController _enemyController;
    public virtual void SetUp(Transform agent){
        _brain = agent.GetComponent<AIBrain>();
        _controller = agent.GetComponent<EnemyController>();
        _transitions = new List<AITransition>();
        _actionData = agent.Find("AD").GetComponent<ActionData>();
        _agentAnimator = agent.Find("Visual").GetComponent<AgentAnimator>();
        _enemyController = agent.GetComponent<EnemyController>();

        GetComponentsInChildren<AITransition>(_transitions);

        foreach(var t in _transitions){
            t.SetUp(agent);
        }
    }
    public abstract void OnEnterState();
    
    public abstract void OnExitState();
    public virtual void OnUpdateState(){
        foreach(var t in _transitions){
            if(t.MakeATransition()){
                _brain.UpdateState(t.nextState);
            }
        }
        _agentAnimator.SetSpeed(_enemyController.NavMeshAgent.velocity.sqrMagnitude);
    }
}