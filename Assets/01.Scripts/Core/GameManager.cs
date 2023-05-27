using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
    public static GameManager Instance;
    public List<WayPoint> wayPoints = new List<WayPoint>();

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }
        else{
            DontDestroyOnLoad(this);
        }

        GetComponentsInChildren<WayPoint>(wayPoints);
    }


    public WayPoint RandomWayPoint(){
        int index = 0;
        index = Random.Range(0,wayPoints.Count);
        return wayPoints[index];
    }
}