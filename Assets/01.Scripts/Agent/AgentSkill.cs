using System.Transactions;
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
    protected AgentHighlighting _agentHighlighting;
    protected Collider _targetCol;
    protected PhotonView _PV;

    protected bool _canAttack;
    [SerializeField] protected Material _changeMat;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected float _radius = 0.8f;

    [SerializeField] SpinStar _spinStar;

    [SerializeField] protected Material _originMat;
    protected Collider _col;
    protected List<SkinnedMeshRenderer> _skins = new List<SkinnedMeshRenderer>();
    protected virtual void Awake() {
        _canAttack = true;
        _agentInput = GetComponent<AgentInput>();
        _col = transform.Find("Collider").GetComponent<Collider>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _actionData = transform.Find("AD").GetComponent<ActionData>();
        _agentMovement = GetComponent<AgentMovement>();
        _agentHighlighting = transform.Find("Collider").GetComponent<AgentHighlighting>();
        _PV = GetComponent<PhotonView>();
    }

    protected virtual void Start() {
        _spinStar.gameObject.SetActive(false);
        _agentInput.OnAttackKeyPress += StartAttack;
        _agentAnimator.OnAttackTrigger += Attack;
        _agentAnimator.OnPenaltyEndTrigger += () => _PV.RPC("ShowSpinStarRPC",RpcTarget.All,false);
        
        _agentAnimator.transform.GetComponentsInChildren<SkinnedMeshRenderer>(_skins);
    }

    protected void Update() {
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
                _targetCol.transform.GetComponent<AgentHighlighting>().SetMaterial(0.03f,Color.white);
            }
            else {
                _targetCol.transform.GetComponent<AgentHighlighting>().SetMaterial(0f,Color.white);
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


    protected virtual void Attack(){
        //Debug.Log($"TargetName: {_targetCol}");
        if (_targetCol != null) {
            //Debug.Log("TargetCol not null");
            if (_targetCol.transform.parent.TryGetComponent<AgentHP>(out AgentHP agentHP)){
                //Debug.Log("Damaged");
                Player player = PhotonNetwork.LocalPlayer;
                if(_targetCol.gameObject.CompareTag("PLAYER")){
                    RoomManager.Instance.UpdateKillCountAndScore(1,100,player.NickName);
                }
                else{
                    _PV.RPC("ShowSpinStarRPC",RpcTarget.All,true);
                    _agentAnimator.SetPeanlty(true);
                    _agentAnimator.SetBoolPenalty(true);
                }
                agentHP.Damaged(player);
            }
        }
    }

    [PunRPC]
    public void ShowSpinStarRPC(bool result){
        _spinStar.gameObject.SetActive(result);
    }

    protected virtual void StartAttack(){
        if(_actionData.IsPenalty) return;
        if(_canAttack == false) return;
        if(_actionData.IsAttacking == false){
            GAME_STATE currentState = RoomManager.Instance.CurrentState;

            if (currentState == GAME_STATE.UI) return;
            if (currentState == GAME_STATE.LOADING) return;
            if(currentState == GAME_STATE.CHAT) return;

            InvisibleItem(true);
            InGameUI.Instance.SetClockUI(0, 0, false);
            if (_targetCol != null) {
                transform.rotation = Quaternion.LookRotation(_targetCol.transform.root.position - transform.position);
            }
            
            _actionData.IsAttacking = true;
            _agentAnimator.SetBoolAttack(true);
            _agentAnimator.SetTriggerAttack();
            _agentMovement.SetRunSpeed(false);
            StartCoroutine(AttackDelayCor(3f));
        }
    }

    public void InvisibleItem(bool result){
        _PV.RPC("InvisibleItemRPC",RpcTarget.All,result);
        //투명이 되는 로직
    }

    [PunRPC]
    public void InvisibleItemRPC(bool result){
        Action<float> action = delegate (float value) {
            foreach (var skin in _skins) {
                MaterialPropertyBlock matProp = new MaterialPropertyBlock();
                skin.GetPropertyBlock(matProp);

                matProp.SetFloat("_Opacity", value);
                skin.SetPropertyBlock(matProp);
                //Debug.LogError("InvisibleItem");
                _agentHighlighting.SetMaterialOpacity(Mathf.Round(value));
            }
        };
        if (_PV.IsMine) {
            if(result){
                action(1f);
            }
            else{
                action(0.3f);
            }
        }
        else {
            if (result) {
                action(1f);
            }
            else {
                action(0f);
            }
        }
    }
    public void SilhouetteItem(bool result){
        _PV.RPC("SilhouetteItemRPC",RpcTarget.All,result);
    }
    //실루엣 아이템을 먹은 사람이 아니면 실행 시켜주어야 함
    [PunRPC]
    public void SilhouetteItemRPC(bool result){
        if(_PV.IsMine == false){
            if(result){
                _agentHighlighting.SetMaterial(0.05f,Color.red);
            }
            else{
                _agentHighlighting.SetMaterial(0f,Color.white);
            }
        }
    }

    IEnumerator AttackDelayCor(float delay = 1f){
        _canAttack = false;
        yield return new WaitForSeconds(delay);
        _canAttack = true;
    }
}