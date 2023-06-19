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
        if(string.IsNullOrEmpty(_mainUI.GetText())){
            return;
        }
        
        _mainUI.SetPlayerVisible(true);
        PhotonNetwork.NickName = _mainUI.GetText();
        _mainUI.SetPlayerNameText(PhotonNetwork.NickName);
        _selectedName = true;
        _mainUI.OpenRoomMenu();
    }

    public void CreateRoom(string roomName = null){

        if(roomName == null){
            roomName = $"RoomName: {Random.Range(1000,9999)}";
        }
        PhotonNetwork.CreateRoom(roomName);
        Debug.Log($"RoomName: {roomName}");
        //MenuManager.Instance.OpenMenu("loading");
    }

    //JoinRoom 했을 때 플레이어 이름 뜨도록 만들어야함
    public override void OnJoinedRoom(){
        _mainUI.OpenInRoomMenu();
        Player[] players = PhotonNetwork.PlayerList;
        RoomInfo roomInfo = PhotonNetwork.CurrentRoom;
        _mainUI.CreatePlayerName(roomInfo,players);
        _mainUI.StartButtonEnabled(PhotonNetwork.IsMasterClient);
        _mainUI.SetRoomNameText(roomInfo.Name);
    }
    public override void OnMasterClientSwitched(Player newMasterClient){
        _mainUI.StartButtonEnabled(PhotonNetwork.IsMasterClient);
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
        Debug.Log("LeftRoom");
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom(){
        Debug.Log("OnLeftRoom");
        _mainUI.OpenInRoomMenu(false);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        Debug.Log("OnRoomListUpdate");
        _mainUI.CreateRoomBtn(roomList,_maxPlayerCount);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        _mainUI.AddName(newPlayer);
        Instantiate(_playerListItemPrefab,_playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}