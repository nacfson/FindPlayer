using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : PoolableMono{
    public override void Init() {
    }

    public Vector3 ReturnPos(){
        return transform.position;
    }
}