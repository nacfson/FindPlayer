using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivedDecision : AIDecision{
    public override bool MakeADecision(){
        return _actionData.IsArrived;
    }
}