using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks{
    [SerializeField] private Transform _canvas;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_Text _errorText;
    [SerializeField] private TMP_Text _roomNameText;

    private Button _startButton;
    
    private void Awake() {
        _startButton = _canvas.Find("Start").GetComponent<Button>();

        Debug.Log("Connecting To Master");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster(){
        Debug.Log("On Connected To Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby(){
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("On Joined Lobby");
    }

    public void CreateRoom(){
        if(string.IsNullOrEmpty(_roomNameInputField.text)){
            return;
        }
        PhotonNetwork.CreateRoom(_roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom(){
        MenuManager.Instance.OpenMenu("room");
        _roomNameText.SetText(PhotonNetwork.CurrentRoom.Name);
    }
    public override void OnCreateRoomFailed(short returnCode, string message){
        string text = $"Room Creation Failed {message}" ;
        _errorText.SetText(text);
        MenuManager.Instance.OpenMenu("error");
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }
    public override void OnLeftRoom(){
        MenuManager.Instance.OpenMenu("title");
    }
}