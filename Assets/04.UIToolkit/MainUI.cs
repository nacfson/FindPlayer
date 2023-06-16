using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;

public class MainUI : MonoBehaviour {
    [SerializeField]
    private VisualTreeAsset _roomBtn;
    [SerializeField]
    private VisualTreeAsset _playerName;

    private UIDocument _uiDocument;
    private TextField _nameField;
    private Label _playerNameLabel;

    private Button _playBtn;
    private VisualElement _roomBtnContainer;
    private VisualElement _playerMenuContainer;

    private VisualElement _inRoomMenu;
    private VisualElement _roomMenu;
    private VisualElement _titleMenu;

    private void Awake() {
        _uiDocument = GetComponent<UIDocument>();   
    }

    private void OnEnable() {
        VisualElement root = _uiDocument.rootVisualElement;
        _titleMenu = root.Q<VisualElement>("TitleMenu");
        _roomMenu = root.Q<VisualElement>("RoomMenu");
        _inRoomMenu = root.Q<VisualElement>("InRoomMenu");

        Button startBtn = _titleMenu.Q<Button>("StartBtn");

        startBtn.RegisterCallback<ClickEvent>(e => {
            //메뉴 로드
            //RoomManager.Instance.InitGame();
            NetworkManager.Instance.JoinLobby();
        });

        Button roomBtn = _roomMenu.Q<Button>("CreateRoomBtn");
        roomBtn.RegisterCallback<ClickEvent>(e=>{
            NetworkManager.Instance.CreateRoom();
        });
        _roomBtnContainer = _roomMenu.Q<VisualElement>("RoomView");
        _playerMenuContainer = _inRoomMenu.Q<VisualElement>("PlayerView");

        _playerNameLabel = _roomMenu.Q<Label>("NameLabel");
        _playBtn = _inRoomMenu.Q<Button>("PlayBtn");
        _nameField = _titleMenu.Q<TextField>("NameField");

        _playBtn.RegisterCallback<ClickEvent>(e => {
            RoomManager.Instance.InitGame();
        });
    }

    public void OpenRoomMenu(bool isRoom = true) {
        if (isRoom) {
            _titleMenu.RemoveFromClassList("active");
            _roomMenu.AddToClassList("active");
        }
        else {
            _roomMenu.RemoveFromClassList("active");
            _titleMenu.AddToClassList("active");
        }
    }
    public void OpenInRoomMenu(bool isRoom = true) {
        if (isRoom) {
            _roomMenu.RemoveFromClassList("active");
            _inRoomMenu.AddToClassList("active");
        }
        else {
            _inRoomMenu.RemoveFromClassList("active");
            _roomMenu.AddToClassList("active");
        }
    }

    public void CreateRoomBtn(List<RoomInfo> roomList,int maxPlayerCount){
        Action<RoomInfo,bool> createBtn = delegate (RoomInfo roomInfo,bool visible){
            VisualElement roomBtn = _roomBtn.Instantiate();
            Button roomButton = roomBtn.Q<Button>("RoomBtn");
            _roomBtnContainer.Add(roomButton);
            Debug.LogError($"RoomName: {roomInfo.Name}");
            roomButton.text = $"{roomInfo.Name} {roomInfo.PlayerCount} / {maxPlayerCount}";
            roomButton.visible = true;
            roomButton.RegisterCallback<ClickEvent>(e =>{
                NetworkManager.Instance.JoinRoom(roomInfo);
            });
        };
        //기존에 있는 버튼 삭제
        _roomBtnContainer.Clear();

        Debug.LogError($"Count: {roomList.Count}");
        for (int i = 0; i < roomList.Count; i++) {
            if (roomList[i].RemovedFromList) {
                Debug.LogError("Continued");
                continue;
            }
            if(roomList[i].PlayerCount >= maxPlayerCount) {
                createBtn(roomList[i],false);
                continue;
            }
            createBtn(roomList[i],true);
        }
    }
    public void CreatePlayerName(RoomInfo roomInfo,Player[] players) {
            //기존에 있던 NameLabel들 삭제
        _playerMenuContainer.Clear();
        for(int i = 0; i < players.Length; i++){
            AddName(players[i]);
        }
    }
    public void AddName(Player player) {
        VisualElement temp = _playerName.Instantiate();
        Label name = temp.Q<Label>("PlayerName");
        _playerMenuContainer.Add(name);
        name.text = player.NickName;
    }
    public void StartButtonEnabled(bool result) {
        _playBtn.SetEnabled(result);
    }
    public string GetText() {
        return _nameField.text;
    }

    public void SetPlayerNameText(string playerName){
        string result = $"{playerName}";
        _playerNameLabel.text = result;
    }
}