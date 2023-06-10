using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Item : MonoBehaviour{
    protected PhotonView _PV;
    public abstract void UseItem(AgentItem agentItem);

    protected virtual void Awake(){
        _PV = GetComponent<PhotonView>();
    }
}