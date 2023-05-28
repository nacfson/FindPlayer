using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentInput : MonoBehaviour{
    public event Action<float, float> OnMouseInput;

    public event Action<Vector3> OnMovementKeyPress;
    public event Action OnAttackKeyPress;
    
    public event Action OnJumpKeyPress;

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
        float x = Input.GetAxisRaw("Horizontal"),z= Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(x,0,z);

        OnMovementKeyPress?.Invoke(dir);
    }
}