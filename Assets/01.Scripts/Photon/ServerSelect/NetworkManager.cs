using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks{
    public static NetworkManager Instance;
    [SerializeField] private Transform _canvas;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_Text _errorText;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private Transform _roomListContent;
    [SerializeField] private Transform _playerListContent;
    [SerializeField] private GameObject _roomListItemPrefab;
    [SerializeField] private GameObject _playerListItemPrefab;
    [SerializeField] private GameObject _startGameButton;
    [SerializeField] private int _maxPlayerCount = 10;
    private Button _startButton;
    
    private void Awake() {
        Instance = this;

        Debug.Log("Connecting To Master");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster(){
        Debug.Log("On Connected To Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true; //씬을 로딩했을때  다같이 이동하게 됨
}

    public override void OnJoinedLobby(){
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("On Joined Lobby");
        PhotonNetwork.NickName = "Player: " + Random.Range(0,1000).ToString("0000");
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


        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in _playerListContent){
            Destroy(child.gameObject);
        }
        for(int i =0 ; i< players.Count(); i++){
            Instantiate(_playerListItemPrefab,_playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient){
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message){
        string text = $"Room Creation Failed {message}" ;
        _errorText.SetText(text);
        MenuManager.Instance.OpenMenu("error");
    }
    public void JoinRoom(RoomInfo info){
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }
    public void StartGame(){
        PhotonNetwork.LoadLevel(1); //Scene Index
    }
    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }
    public override void OnLeftRoom(){
        MenuManager.Instance.OpenMenu("title");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        foreach(Transform trans in _roomListContent){
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++) {
            if (roomList[i].RemovedFromList) {
                continue;
            }
            if(roomList[i].PlayerCount >= _maxPlayerCount) {
                Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i],_maxPlayerCount,false);
                continue;
            }
            Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i], _maxPlayerCount);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer){
        Instantiate(_playerListItemPrefab,_playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

}