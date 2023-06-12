using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class InvisibleItem : Item{
    [SerializeField] private float _invisibleTime = 10f;
    private bool _canUse = true;
    public override void UseItem(AgentItem agentItem){
        if(_canUse == false) return;
        InGameUI.Instance.OnClockEnd += StopAllCoroutines;
        StartCoroutine(Invisible(agentItem));
    }

    IEnumerator Invisible(AgentItem agentItem){
        float timer = 0f;
        _canUse = false;
        agentItem.agentSkill.InvisibleItem(false);
        while(timer < _invisibleTime){
            timer += Time.deltaTime;
            InGameUI.Instance.SetClockUI(_invisibleTime - timer,_invisibleTime);
            yield return null;
        }
        InGameUI.Instance.SetClockUI(0, 0, false);
        agentItem.agentSkill.InvisibleItem(true);
    }
}