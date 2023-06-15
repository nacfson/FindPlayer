using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks{
    public static NetworkManager Instance;

    public List<RoomInfo> roomListNet = new List<RoomInfo>();

    [SerializeField] private Transform _canvas;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_InputField _nickNameInputField;
    [SerializeField] private TMP_Text _errorText;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private Transform _roomListContent;
    [SerializeField] private Transform _playerListContent;
    [SerializeField] private GameObject _roomListItemPrefab;
    [SerializeField] private GameObject _playerListItemPrefab;
    [SerializeField] private GameObject _startGameButton;
    [SerializeField] private int _maxPlayerCount = 10;
    [SerializeField] private MainUI _mainUI;
    private PhotonView _PV;

    private bool _selectedName; 
    private Button _startButton;
    
    private void Awake() {
        Instance = this;
        
        _selectedName = false;
        _PV = GetComponent<PhotonView>();
        Debug.Log("Connecting To Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        Debug.Log("On Connected To Master");
        if(_selectedName == false){
            MenuManager.Instance.OpenMenu("selectName");
        }
        PhotonNetwork.AutomaticallySyncScene = true; //한 명이 씬을 로딩했을때  다같이 이동하게 됨
    }

    public void JoinLobby() {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
         //MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedLobby(){
        //MenuManager.Instance.OpenMenu("title");
        Debug.Log("On Joined Lobby");
        PhotonNetwork.NickName = _mainUI.GetText();
        _selectedName = true;
    }

    public void CreateRoom(){
        if(string.IsNullOrEmpty(_roomNameInputField.text)){
            return;
        }
        PhotonNetwork.CreateRoom(_roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom(){
        RoomInfo roomInfo = PhotonNetwork.CurrentRoom;
        MenuManager.Instance.OpenMenu("room");
        _roomNameText.SetText(PhotonNetwork.CurrentRoom.Name);
        
        PhotonNetwork.CurrentRoom.IsVisible = true;
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
        roomListNet = roomList;
        foreach(Transform trans in _roomListContent){
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++) {
            if (roomList[i].RemovedFromList) {
                continue;
            }
            if(roomList[i].PlayerCount >= _maxPlayerCount) {
                Instantiate(_roomListItemPrefab, _roomListContent)
                    .GetComponent<RoomListItem>().SetUp(roomList[i],_maxPlayerCount,false);
                continue;
            }
            Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i], _maxPlayerCount);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        Instantiate(_playerListItemPrefab,_playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }


}