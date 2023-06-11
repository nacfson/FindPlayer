using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour{
    public Item item;
    private Transform _getTransform;
    [SerializeField] private LayerMask _layerMask;
    private void Awake() {
        _getTransform = this.transform;
    }
    void Update(){
        bool result = Physics.Raycast(_getTransform.position,Vector3.up,out RaycastHit hit,10f,_layerMask);

        if(result){
            if(hit.collider.transform.parent.TryGetComponent<AgentItem>(out AgentItem agentItem)){
                agentItem.UseItem(() => item.UseItem(agentItem));
            }
        }
    }
    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawRay(_getTransform.position, Vector3.up * 10f);
    //}
}