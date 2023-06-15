using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;

public class MainUI : MonoBehaviour {
    [SerializeField]
    private VisualTreeAsset _roomBtn;

    private UIDocument _uiDocument;
    private TextField _nameField;
    private Label _playerNameLabel;
    private VisualElement _roomBtnContainer;
    private VisualElement _roomMenu;
    private VisualElement _titleMenu;

    private void Awake() {
        _uiDocument = GetComponent<UIDocument>();   
    }

    private void OnEnable() {
        VisualElement root = _uiDocument.rootVisualElement;
        _titleMenu = root.Q<VisualElement>("TitleMenu");
        _roomMenu = root.Q<VisualElement>("RoomMenu");

        Button startBtn = _titleMenu.Q<Button>("StartBtn");

        startBtn.RegisterCallback<ClickEvent>(e => {
            //메뉴 로드
            //RoomManager.Instance.InitGame();
            NetworkManager.Instance.JoinLobby();
        });

        Button roomBtn = _roomMenu.Q<Button>("CreateRoomBtn");
        roomBtn.RegisterCallback<ClickEvent>(e=>{
            NetworkManager.Instance.CreateRoom();
            Debug.Log("CreateRoom");
        });
        _roomBtnContainer = root.Q<VisualElement>("RoomView");

        _playerNameLabel = root.Q<Label>("NameLabel");
        _nameField = root.Q<TextField>("NameField");
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

    public void CreateRoomBtn(List<RoomInfo> roomList,int maxPlayerCount){
        Action<RoomInfo,bool> createBtn = delegate (RoomInfo roomInfo,bool visible){
            VisualElement roomBtn = _roomBtn.Instantiate();
            Button roomButton = roomBtn.Q<Button>("RoomBtn");
            roomButton.text = $"{roomInfo.Name} {roomInfo.PlayerCount} / {maxPlayerCount}";
            roomButton.visible = true;
            Debug.Log("CreatedRoomButton");
            roomButton.RegisterCallback<ClickEvent>(e =>{
                NetworkManager.Instance.JoinRoom(roomInfo);
            });
        };
        for(int i = 0; i < _roomBtnContainer.childCount; i++){
            _roomBtnContainer[i].RemoveFromHierarchy();
        }
        for (int i = 0; i < roomList.Count; i++) {
            if (roomList[i].RemovedFromList) {
                continue;
            }
            if(roomList[i].PlayerCount >= maxPlayerCount) {
                createBtn(roomList[i],false);
                continue;
            }
            createBtn(roomList[i],true);
        }
    }

    public string GetText() {
        return _nameField.text;
    }

    public void SetPlayerNameText(string playerName){
        string result = $"{playerName}";
        _playerNameLabel.text = result;
    }
}