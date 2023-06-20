using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChatSystem : MonoBehaviour{
    [SerializeField] private ChatSystem _chatSystem;

    void Update(){
        if(Input.GetKeyDown(KeyCode.T)){
            if(_chatSystem.IsActive()){
                _chatSystem.UnShowingSequence();
            }
            else{
                _chatSystem.ShowingSequence();
            }
        }
    }




}