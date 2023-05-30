using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour{
    private NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    private ActionData _actionData;

    private void Awake() {
        _navMeshAgent  = GetComponent<NavMeshAgent>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
    }

    public void SetDestination(Vector3 pos){
        _navMeshAgent.SetDestination(pos);
    }

    public void EnableNavMesh(bool result) {
        _navMeshAgent.enabled = result;
    }

    public void StopImmediately(){
        _navMeshAgent.SetDestination(transform.position);
        _navMeshAgent.velocity = Vector3.zero;
    }
    void Update(){
        bool result = Vector3.Distance(transform.position , _navMeshAgent.destination) < 0.1f;
        _actionData.IsArrived = result;
    }
    public bool IsMoving(){
        if(_navMeshAgent.velocity.sqrMagnitude >= 0.2f * 0.2f){
            return true;
        }
        return false;
    }
}