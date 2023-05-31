using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AgentSkill : MonoBehaviourPunCallbacks{
    protected AgentInput _agentInput;
    protected AgentAnimator _agentAnimator;
    protected ActionData _actionData;
    protected AgentMovement _agentMovement;
    protected Collider _targetCol;

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
    }

    private void Start() {
        _agentInput.OnAttackKeyPress += StartAttack;
        _agentAnimator.OnAttackTrigger += Attack;
    }
    private void Update() {
        if(_actionData.IsAttacking) return;
        Collider temp = GetClosestObjectCollider();

        if(_targetCol != temp && temp != null) {
            if(_targetCol != null) {
                SettingTargetObj(false);
            }
            _targetCol = temp;
            SettingTargetObj(true);
        }
        else if(_targetCol != null) {
            SettingTargetObj(true);
        }
    }

    public void SettingTargetObj(bool result) {
        if(result) {
            _originMat = _targetCol.transform.GetComponent<AgentHighlighting>().GetMaterial();
            _targetCol.transform.GetComponent<AgentHighlighting>().SetMaterial(_changeMat);
        }
        else {
            _targetCol.transform.GetComponent<AgentHighlighting>().SetMaterial(_originMat);
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
            if (_targetCol.TryGetComponent<AgentHP>(out AgentHP agentHP)){
                agentHP.Damaged();
            }

        }
    }
    private void StartAttack(){
        if(_actionData.IsAttacking == false){
            if(_targetCol != null) {
                transform.rotation = Quaternion.LookRotation(_targetCol.transform.root.position - transform.position);
            }
            
            _actionData.IsAttacking = true;
            _agentAnimator.SetBoolAttack(true);
            _agentAnimator.SetTriggerAttack();
            _agentMovement.SetRunSpeed(false);
        }
    }
}