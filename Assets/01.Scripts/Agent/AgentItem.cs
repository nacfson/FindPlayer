using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentItem : MonoBehaviour{
    public AgentAnimator agentAnimator;
    public AgentMovement agentMovement;
    public AgentController agentController;
    public AgentSkill agentSkill;

    void Awake(){
        agentSkill = GetComponent<AgentSkill>();
        agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        agentMovement = GetComponent<AgentMovement>();
        agentController = GetComponent<AgentController>();
    }
    public void UseItem(Action action = null){
        if(action == null){
            Debug.LogError("I can't find that I'll play action!");
        }
        else{
            action?.Invoke();
        }
    }
}