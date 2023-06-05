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
        _agentAnimator = transform.parent.Find("Visual").GetComponent<AgentAnimator>();
        _actionData = transform.parent.Find("AD").GetComponent<ActionData>();
        _PV = transform.root.GetComponent<PhotonView>();
        _cmCam = GetComponentInChildren<CinemachineVirtualCamera>();
        _agentController = transform.parent.GetComponent<AgentController>();
        _agentCamera = transform.parent.GetComponent<AgentCamera>();
    }
    public void Damaged(Player attacker){
        DeadProcess(attacker);

    }

    protected virtual void DeadProcess(Player attacker){
        _agentAnimator.OnDead(true);
        OnDead?.Invoke();
        RoomManager.Instance.UpdateKillCountAndScore(1,30);
        Debug.Log("DeadProcess");
    }
}