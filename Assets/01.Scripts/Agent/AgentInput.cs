using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using Photon.Pun;
using Core;

public class AgentInput : MonoBehaviour{
    public event Action<float, float> OnMouseInput;

    public event Action<Vector3> OnMovementKeyPress;
    public event Action OnAttackKeyPress;
    public event Action OnMouseClicked;
    
    public event Action OnJumpKeyPress;
    protected ActionData _actionData;

    protected PhotonView _PV;

    private void Awake() {
        _actionData = transform.Find("AD").GetComponent<ActionData>();
        _PV = GetComponent<PhotonView>();
    }
    private void Update() {

        GetMovementInput();
        GetJumpInput();
        GetAttackInput();
        GetMouseInput();
    }



    private void GetMouseInput(){
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        OnMouseInput?.Invoke(x,y);
    }

    private void GetAttackInput(){
        if(Input.GetMouseButton(0)){
            OnAttackKeyPress?.Invoke();
        }
    }

    private void GetJumpInput(){
        if(Input.GetButtonDown("Jump")){
            OnJumpKeyPress?.Invoke();
        }
    }
    private void GetMovementInput(){
        if(_actionData.IsAttacking) return;
        float x = Input.GetAxis("Horizontal"),z= Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(x,0,z);

        OnMovementKeyPress?.Invoke(dir);
    }
}