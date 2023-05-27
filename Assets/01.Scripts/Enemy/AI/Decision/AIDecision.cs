using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AIDecision : MonoBehaviour{
    public bool IsReverse;
    protected AIBrain _brain;
    protected ActionData _actionData;
    public virtual void SetUp(Transform agent){
        _brain = agent.GetComponent<AIBrain>();
        _actionData = agent.Find("AD").GetComponent<ActionData>();
    }
    public abstract bool MakeADecision();
}