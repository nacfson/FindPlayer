using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
    public static GameManager Instance;
    public List<WayPoint> wayPoints = new List<WayPoint>();
    [SerializeField] private Vector3 _originOffset;
    
    private void Awake() {
        if(Instance == null){
            Instance = this;
        }
        else{
            DontDestroyOnLoad(this);
        }

        GetComponentsInChildren<WayPoint>(wayPoints);
        MakeWayPoints();
    }

    private void MakeWayPoints(){
        for(int i = 0; i < 50; i++){
            for(int j =0 ; j < 50; j++){
                Vector3 offset = _originOffset + new Vector3(i,0,j);
            }
        }
    }

    public WayPoint RandomWayPoint(){
        int index = 0;
        index = Random.Range(0,wayPoints.Count);
        return wayPoints[index];
    }
}