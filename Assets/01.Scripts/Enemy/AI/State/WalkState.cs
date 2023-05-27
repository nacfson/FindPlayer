using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : State{
    private WayPoint _wayPoint;
    [SerializeField] private TimerData _timerSO;
    public override void OnEnterState(){
        _wayPoint = GameManager.Instance.RandomWayPoint();
        _controller.SetDestination(_wayPoint.ReturnPos());
        _agentAnimator.SetSpeed(1f);
        StartCoroutine(DelayCoroutine(_timerSO.ReturnRandomTime()));
    }

    public override void OnExitState(){
        _agentAnimator.SetSpeed(0f);
        StopAllCoroutines();
    }

    public override void OnUpdateState(){
        base.OnUpdateState();
    }

    IEnumerator DelayCoroutine(float delayTime){
        float timer = 0f;
        while(timer < delayTime){
            timer += Time.deltaTime;            
            yield return null;
        }
        _controller.StopImmediately();
    }
}