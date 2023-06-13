using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilhouetteItem : Item {
    [SerializeField] private float _radius = 100f;
    [SerializeField] private float _silhouetteTime  = 5f;

    public override void UseItem(AgentItem agentItem) {
        if (_canUse == false) return;
        Collider[] cols = Physics.OverlapSphere(agentItem.transform.position,_radius, 1 << LayerMask.NameToLayer("COLLIDER"));

        foreach(Collider col in cols) {
            if (col == agentItem.collider) continue;
            if(col.transform.parent.TryGetComponent<AgentSkill>(out AgentSkill agentSkill)){
                agentSkill.SilhouetteItem(true);
                StartCoroutine(SilhouetteCor(_silhouetteTime,agentSkill));
            }
        }

    }

    private IEnumerator SilhouetteCor(float delay,AgentSkill agentSkill){
        float timer = 0f;
        while(timer < delay){
            timer += Time.deltaTime;
            yield return null;
        }
        agentSkill.SilhouetteItem(false);
    }
}