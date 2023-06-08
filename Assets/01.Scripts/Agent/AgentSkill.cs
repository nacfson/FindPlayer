using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AgentSkill : MonoBehaviourPunCallbacks{
    protected AgentInput _agentInput;
    protected AgentAnimator _agentAnimator;
    protected ActionData _actionData;
    protected AgentMovement _agentMovement;
    protected Collider _targetCol;
    protected PhotonView _PV;

    [SerializeField] protected Material _changeMat;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected float _radius = 0.8f;

    [SerializeField] protected Material _originMat;
    protected Collider _col;
    private void Awake() {
        _agentInput = GetComponent<AgentInput>();
        _col = transform.Find("Collider").GetComponent<Collider>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
        _agentMovement = GetComponent<AgentMovement>();
        _PV = GetComponent<PhotonView>();
    }

    private void Start() {
        _agentInput.OnAttackKeyPress += StartAttack;
        _agentAnimator.OnAttackTrigger += Attack;
    }
    private void Update() {
        if(_actionData.IsAttacking) return;


        if(_targetCol != null) {
            SettingTargetObj(false);
        }
        Collider temp = GetClosestObjectCollider();
        _targetCol = temp;
        if(_targetCol != null) {
            SettingTargetObj(true);
        }
    }

    public void SettingTargetObj(bool result) {
        if (_PV.IsMine) {
            if (result) {
                _targetCol.transform.GetComponent<AgentHighlighting>().SetMaterial(0.02f);
            }
            else {
                _targetCol.transform.GetComponent<AgentHighlighting>().SetMaterial(0f);
            }
        }
    }

    public Collider GetClosestObjectCollider() {
        Collider closestCollider = null;
        float closestDistance = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius,_layerMask);

        foreach (Collider collider in colliders) {
            if(_col == collider) {
                continue;
            }
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestCollider = collider;
            }
        }
        return closestCollider;
    }

    private void Attack(){
        Debug.Log($"TargetName: {_targetCol}");
        if (_targetCol != null) {
            Debug.Log("TargetCol not null");
            if (_targetCol.transform.parent.TryGetComponent<AgentHP>(out AgentHP agentHP)){
                Debug.Log("Damaged");
                Player player = PhotonNetwork.LocalPlayer;
                if(_targetCol.gameObject.CompareTag("PLAYER")){
                    RoomManager.Instance.UpdateKillCountAndScore(1,100,player.NickName);
                }
                agentHP.Damaged(player);
            }
        }
    }
    private void StartAttack(){
        if(_actionData.IsAttacking == false){
            GAME_STATE currentState = RoomManager.Instance.CurrentState;

            if (currentState == GAME_STATE.UI) return;
            if (currentState == GAME_STATE.LOADING) return;

            if (_targetCol != null) {
                transform.rotation = Quaternion.LookRotation(_targetCol.transform.root.position - transform.position);
            }
            
            _actionData.IsAttacking = true;
            _agentAnimator.SetBoolAttack(true);
            _agentAnimator.SetTriggerAttack();
            _agentMovement.SetRunSpeed(false);
        }
    }
}