using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameManager : MonoBehaviourPunCallbacks{
    public static GameManager Instance;
    public List<WayPoint> wayPoints = new List<WayPoint>();
    [SerializeField] private Vector3 _originOffset;
    [SerializeField] private PoolingListSO _poolingListSO;

    [SerializeField] private WayPoint _wayPoint;
    [SerializeField] private AIBrain _aiPlayer;

    [SerializeField] private int _initAICount = 30;
    
    private void Awake() {
        if(Instance == null){
            Instance = this;
        }
        else{
            DontDestroyOnLoad(this);
        }
        //CreatePool();
        GetComponentsInChildren<WayPoint>(wayPoints);
        MakeAIPlayer();
    }
    private void CreatePool() {
        PoolManager.Instance = new PoolManager(this.transform);

        foreach(var p in _poolingListSO.pairs) {
            PoolManager.Instance.CreatePool(p.prefab,p.count);
        }
    }
    private void MakeAIPlayer(){
        for(int i = 0; i < _initAICount; i++) {
            AIBrain brain = Instantiate<AIBrain>(_aiPlayer);
            brain.EnemyController.EnableNavMesh(false);
            brain.transform.position = RandomWayPoint().ReturnPos();
            brain.EnemyController.EnableNavMesh(true);

        }
    }

    public WayPoint RandomWayPoint(){
        int index = 0;
        index = Random.Range(0,wayPoints.Count);
        return wayPoints[index];
    }
}