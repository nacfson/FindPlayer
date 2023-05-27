using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State{
    [SerializeField] private TimerData _idleTimer;
    private float _targetTime;
    
    public override void OnEnterState(){
        _targetTime = _idleTimer.ReturnRandomTime();
        _actionData.CanMove = false;
        _controller.StopImmediately();
        
        StartCoroutine(DelayCoroutine(_targetTime));
    }

    public override void OnExitState(){
        StopAllCoroutines();
    }

    public override void OnUpdateState(){
        base.OnUpdateState();

    }
    IEnumerator DelayCoroutine(float targetTime){
        float timer = 0f;
        while(timer < targetTime){
            Debug.Log($"Percent: {timer / targetTime}");
            Debug.Log($"CanMove: {_actionData.CanMove}");
            timer += Time.deltaTime;
            yield return null;
        }
        _actionData.CanMove = true;
        OnUpdateState();
    }
}