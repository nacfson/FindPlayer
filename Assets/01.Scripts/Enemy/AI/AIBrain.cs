using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : PoolableMono{
    public State CurrentState => _currentState;
    public EnemyController EnemyController => _enemyController;

    private EnemyController _enemyController;
    [SerializeField] private State _currentState;

    private List<State> _states;

    private void Awake() {
        _states = new List<State>();
        _enemyController = GetComponent<EnemyController>();
        transform.Find("AD").GetComponentsInChildren<State>(_states);

        foreach(var s in _states){
            s.SetUp(this.transform);
        }
    }
    private void Start() {
        _currentState.OnEnterState();
    }

    private void Update() {
        if(RoomManager.Instance.CurrentState == GAME_STATE.LOADING) return;
        _currentState.OnUpdateState();
    }

    public void UpdateState(State state){
        _currentState.OnExitState();
        this._currentState = state;
        _currentState.OnEnterState();
    }

    public override void Init() {
    }
}