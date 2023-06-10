using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class InvisibleItem : Item{
    [SerializeField] private float _invisibleTime = 5f;
    private bool _canUse = true;
    public override void UseItem(AgentItem agentItem){
        if(_canUse == false) return;
        StartCoroutine(Invisible(agentItem));
    }

    IEnumerator Invisible(AgentItem agentItem){
        float timer = 0f;
        _canUse = false;
        agentItem.agentSkill.InvisibleItem(false);
        while(timer < _invisibleTime){
            timer += Time.deltaTime;
            yield return null;
        }
        agentItem.agentSkill.InvisibleItem(true);
    }
}