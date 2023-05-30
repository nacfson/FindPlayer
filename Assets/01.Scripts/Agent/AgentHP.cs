using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
public class AgentHP : MonoBehaviourPunCallbacks{
    public UnityEvent OnDead;
    protected AgentAnimator _agentAnimator;
    PhotonView PV;
    private void Awake() {
        _agentAnimator = transform.root.Find("Visual").GetComponent<AgentAnimator>();
        PV = transform.root.GetComponent<PhotonView>();
    }
    public void Damaged(){
        DeadProcess();
    }

    private void DeadProcess(){
        _agentAnimator.OnDead(true);
        Debug.Log("DeadProcess");
        OnDead?.Invoke();
    }
}