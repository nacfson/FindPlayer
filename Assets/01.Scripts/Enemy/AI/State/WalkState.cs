using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State{
    private WayPoint _wayPoint;
    [SerializeField] private TimerData _timerSO;
    public override void OnEnterState(){
        _wayPoint = GameManager.Instance.RandomWayPoint();
        _controller.SetDestination(_wayPoint.ReturnPos());
        StartCoroutine(DelayCoroutine(_timerSO.ReturnRandomTime()));
        _actionData.IsArrived = false;
    }

    public override void OnExitState(){
        StopAllCoroutines();
    }

    public override void OnUpdateState(){
        base.OnUpdateState();
        if(Vector3.Distance(_enemyController.NavMeshAgent.destination,transform.position) < 0.1f){
            _actionData.IsArrived = true;
        }
    }
    IEnumerator DelayCoroutine(float delayTime){
        float timer = 0f;
        while(timer < delayTime){
            timer += Time.deltaTime;            
            yield return null;
        }
        _controller.StopImmediately();
        _actionData.IsArrived = true;
    }
}