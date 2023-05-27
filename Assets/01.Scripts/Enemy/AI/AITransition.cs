using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITransition : MonoBehaviour{
    private AIBrain _brain;
    public State nextState;
    private List<AIDecision> _decisions;

    public void SetUp(Transform agent){
        _brain = agent.GetComponent<AIBrain>();
        _decisions = new List<AIDecision>();
        GetComponents<AIDecision>(_decisions);
        foreach(var d in _decisions){
            d.SetUp(agent);
        }
    }
    public bool MakeATransition(){
        bool result = false;
        foreach(var d in _decisions){
            result = d.MakeADecision();
            if(d.IsReverse){
                result = !result;
            }
            if(result == false){
                break;
            }
        }
        return result;
    }
}