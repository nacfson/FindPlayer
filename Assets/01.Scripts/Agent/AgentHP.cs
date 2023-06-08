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
    protected ActionData _actionData;
    protected AgentController _agentController;
    protected AgentCamera _agentCamera;
    protected void Awake() {
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
        _PV = transform.GetComponent<PhotonView>();
        //_cmCam = GetComponentInChildren<CinemachineVirtualCamera>();
        _agentController = transform.GetComponent<AgentController>();
        _agentCamera = transform.GetComponent<AgentCamera>();
    }
    public void Damaged(Player attacker) {
        _PV.RPC("DamagedRPC", RpcTarget.All, attacker);
    }
    [PunRPC]
    public void DamagedRPC(Player attacker){
        if (_PV.IsMine) {
            DeadProcess(attacker);
        }
    }
    protected virtual void DeadProcess(Player attacker){
        _agentAnimator.OnDead(true);
        OnDead?.Invoke();
        Debug.Log("DeadProcess");
    }
}