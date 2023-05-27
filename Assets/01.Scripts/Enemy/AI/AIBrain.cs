using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour{
    public State CurrentState => _currentState;
    [SerializeField] private State _currentState;

    private List<State> _states;

    private void Awake() {
        _states = new List<State>();
        transform.Find("AD").GetComponentsInChildren<State>(_states);

        foreach(var s in _states){
            s.SetUp(this.transform);
        }
    }
    private void Start() {
        _currentState.OnEnterState();
    }

    private void Update() {
        _currentState.OnUpdateState();
    }

    public void UpdateState(State state){
        _currentState.OnExitState();
        this._currentState = state;
        _currentState.OnEnterState();
    }
}