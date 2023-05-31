using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using Photon.Realtime;
using Photon.Pun;
public class AgentHP : MonoBehaviourPunCallbacks{
    public UnityEvent OnDead;
    protected AgentAnimator _agentAnimator;
    protected CinemachineVirtualCamera _cmCam;
    protected PhotonView _PV;
    protected void Awake() {
        _agentAnimator = transform.root.Find("Visual").GetComponent<AgentAnimator>();
        _PV = transform.root.GetComponent<PhotonView>();
        _cmCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }
    public void Damaged(){
        DeadProcess();
    }

    protected virtual void DeadProcess(){
        _agentAnimator.OnDead(true);
        Debug.Log("DeadProcess");
        OnDead?.Invoke();



    }
}