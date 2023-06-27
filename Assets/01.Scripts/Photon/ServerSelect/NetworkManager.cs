using System.Net.WebSockets;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using Core;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks{
    public static NetworkManager Instance;

    public List<RoomInfo> roomListNet = new List<RoomInfo>();
    public Dictionary<WayPoint,SitPlayer> sitPoints = new Dictionary<WayPoint,SitPlayer>();
    [SerializeField]
    private SitPlayer _sitCharacter;

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
    [SerializeField] private ErrorText _error;
    [SerializeField] private Camera _roomCam;

    private PhotonView _PV;

    private bool _selectedName; 
    private Button _startButton;
    
    private void Awake() {
        Instance = this;

        _selectedName = false;
        _PV = GetComponent<PhotonView>();
        Debug.Log("Connecting To Master");
        PhotonNetwork.ConnectUsingSettings();

        List<WayPoint> sitPoints = new List<WayPoint>();
        transform.Find("SitPoint").GetComponentsInChildren<WayPoint>(sitPoints);

        foreach(var s in sitPoints){
            SitPlayer obj = Instantiate<SitPlayer>(_sitCharacter);
            obj.transform.position = s.ReturnPos();
            obj.gameObject.SetActive(false);
            obj.IsEnabled = false;
            this.sitPoints.Add(s,obj);
        }

        Define.MainCam.enabled = true;
        _roomCam.enabled = false;
    }

    public override void OnConnectedToMaster(){
        Debug.Log("On Connected To Master");
        if(_selectedName == false){
            //MenuManager.Instance.OpenMenu("selectName");
        }
        PhotonNetwork.AutomaticallySyncScene = true; //한 명이 씬을 로딩했을때  다같이 이동하게 됨
    }

    public void JoinLobby() {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
         //MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedLobby(){
        Action<string> errorAction = (text) => {
            ErrorText errorText = Instantiate<ErrorText>(_error);
            errorText.transform.SetParent(_canvas);
            errorText.ShowingSequence(text);
        };
        
        if(string.IsNullOrEmpty(_mainUI.GetText())){
            errorAction("최소 1글자의 단어가 들어가야합니다!");
            Debug.Log("NullName");
            PhotonNetwork.LeaveLobby();
            return;
        }
        if(_mainUI.GetText().Length > 8){
            errorAction("8글자가 최대 이름 크기입니다!");
            Debug.Log("MaxName");
            PhotonNetwork.LeaveLobby();
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
        
        Define.MainCam.enabled = false;
        _roomCam.enabled = true;
        DisappearCharacterAll();
        ShowSitCharacterAll(players.ToList());
    }
    public override void OnMasterClientSwitched(Player newMasterClient){
        _mainUI.StartButtonEnabled(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message){
        string text = $"Room Creation Failed {message}" ;
        _errorText.SetText(text);
        //MenuManager.Instance.OpenMenu("error");
    }
    public void JoinRoom(RoomInfo info){
        PhotonNetwork.JoinRoom(info.Name);
        //MenuManager.Instance.OpenMenu("loading");
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
        
        Define.MainCam.enabled = true;
        _roomCam.enabled = false;
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        Debug.Log("OnRoomListUpdate");
        _mainUI.CreateRoomBtn(roomList,_maxPlayerCount);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        _mainUI.AddName(newPlayer);
        Instantiate(_playerListItemPrefab,_playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);

        List<Player> players = PhotonNetwork.PlayerList.ToList();

        DisappearCharacterAll();
        ShowSitCharacterAll(players);
    }
    
    public void ShowSitCharacterAll(List<Player> playerList){
        foreach(var p in playerList){
            WayPoint wayPoint = GetRandomSitPoint();

            SitPlayer sp = sitPoints[wayPoint];
            sp.gameObject.SetActive(true);
            sp.IsEnabled = true;
            sp.SetNickName(p.NickName);
        }
    }

    public void DisappearCharacterAll(){
        foreach(var s in sitPoints){
            if(s.Value.IsEnabled == true){
                s.Value.gameObject.SetActive(false);
                s.Value.IsEnabled = false;
            }
        }
    }

    private WayPoint GetRandomSitPoint(){
        foreach(var s in sitPoints){
            if(s.Value.IsEnabled == false){
                return s.Key;
            }
        }
        Debug.LogError("Can't Find More SitPoint");
        return null;
    }
}

