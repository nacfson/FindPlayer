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
    private void Start() {
        _navMeshAgent.enabled = true;
    }
    public void SetDestination(Vector3 pos){
        if(_navMeshAgent.enabled == true){
            _navMeshAgent.SetDestination(pos);
        }
    }

    public void EnableNavMesh(bool result) {
        _navMeshAgent.enabled = result;
    }

    public void StopImmediately(){
        if(_navMeshAgent.enabled == true){
            _navMeshAgent.SetDestination(transform.position);
            _navMeshAgent.velocity = Vector3.zero;
        }
    }
    void Update(){
        if(RoomManager.Instance.CurrentState == GAME_STATE.LOADING) {
            _navMeshAgent.enabled = false;
        }
        else{
            _navMeshAgent.enabled = true;
        }
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