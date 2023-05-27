using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Timer")]
public class TimerData : ScriptableObject{
    public float MinTime;
    public float MaxTime;

    public float ReturnRandomTime(){
        return Random.Range(MinTime,MaxTime);
    }
}