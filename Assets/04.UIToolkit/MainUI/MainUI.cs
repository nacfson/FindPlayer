using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;

public class MainUI : MonoBehaviour {
    [SerializeField]
    private VisualTreeAsset _roomBtn;
    [SerializeField]
    private VisualTreeAsset _playerName;

    private UIDocument _uiDocument;
    private TextField _nameField;
    private Label _playerNameLabel;
    private Label _roomNameLabel;

    private Button _playBtn;
    private Button _exitRoomBtn;
    private Button _optionOKBtn;
    private Button _optionNOBtn;
    private VisualElement _character;
    
    private List<Button> _exitBtns;
    private List<VisualElement> _menus;

    private VisualElement _roomBtnContainer;
    private VisualElement _playerMenuContainer;

    private VisualElement _inRoomMenu;
    private VisualElement _optionMenu;
    private VisualElement _roomMenu;
    private VisualElement _titleMenu;

    private void Awake() {
        _uiDocument = GetComponent<UIDocument>();   
    }

    private void OnEnable() {
        Init();
    }
    private void Init() {
        VisualElement root = _uiDocument.rootVisualElement;
        _character = root.Q<VisualElement>("Character");
        _character.visible = false;

        _menus = new List<VisualElement>();
        _exitBtns = new List<Button>();
        _titleMenu = root.Q<VisualElement>("TitleMenu");
        _roomMenu = root.Q<VisualElement>("RoomMenu");
        _inRoomMenu = root.Q<VisualElement>("InRoomMenu");
        _optionMenu = root.Q<VisualElement>("OptionMenu");

        _menus.Add(_titleMenu);
        _menus.Add(_roomMenu);
        _menus.Add(_inRoomMenu);

        //_menus.Add(_optionMenu);

        Button startBtn = _titleMenu.Q<Button>("StartBtn");

        startBtn.RegisterCallback<ClickEvent>(e => {
            //메뉴 로드
            //RoomManager.Instance.InitGame();
            UISoundManager.Instance.PlayClickAudio();
            NetworkManager.Instance.JoinLobby();
        });

        Button roomBtn = _roomMenu.Q<Button>("CreateRoomBtn");
        roomBtn.RegisterCallback<ClickEvent>(e => {
            UISoundManager.Instance.PlayClickAudio();

            NetworkManager.Instance.CreateRoom();
        });
        _roomBtnContainer = _roomMenu.Q<VisualElement>("RoomView");
        _playerMenuContainer = _inRoomMenu.Q<VisualElement>("PlayerView");


        _playerNameLabel = _roomMenu.Q<Label>("NameLabel");
        _roomNameLabel = _inRoomMenu.Q<Label>("RoomName");
        _playBtn = _inRoomMenu.Q<Button>("PlayBtn");
        _exitRoomBtn = _inRoomMenu.Q<Button>("ExitRoomBtn");
        _optionOKBtn = _optionMenu.Q<Button>("OKBtn");
        _optionNOBtn = _optionMenu.Q<Button>("NOBtn");

        foreach (var menu in _menus) {
            Button exitBtn = menu.Q<Button>("ExitBtn");

            exitBtn.RegisterCallback<ClickEvent>(e => {
                UISoundManager.Instance.PlayClickAudio();

                _optionMenu.AddToClassList("active");
            });
        }
        _nameField = _titleMenu.Q<TextField>("NameField");


        _playBtn.RegisterCallback<ClickEvent>(e => {
            UISoundManager.Instance.PlayClickAudio();
            RoomManager.Instance.InitGame();
        });
        _exitRoomBtn.RegisterCallback<ClickEvent>(e => {
            UISoundManager.Instance.PlayClickAudio();

            NetworkManager.Instance.LeaveRoom();
        });
        _optionOKBtn.RegisterCallback<ClickEvent>(e => {
            UISoundManager.Instance.PlayClickAudio(() => Application.Quit());
        });
        _optionNOBtn.RegisterCallback<ClickEvent>(e => {
            UISoundManager.Instance.PlayClickAudio();

            _optionMenu.RemoveFromClassList("active");
        });
    }
    public void SetPlayerVisible(bool result) {
        _character.visible = result;
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
    public void GameInit() {
        Init();
        foreach(var m in _menus) {
            m.RemoveFromClassList("active");
        }
        _roomMenu.RemoveFromClassList("active");
        _titleMenu.AddToClassList("active");
    }

    public void CreateRoomBtn(List<RoomInfo> roomList,int maxPlayerCount){
        Action<RoomInfo,bool> createBtn = delegate (RoomInfo roomInfo,bool visible){
            VisualElement roomBtn = _roomBtn.Instantiate();
            Button roomButton = roomBtn.Q<Button>("RoomBtn");
            _roomBtnContainer.Add(roomButton);
            roomButton.text = $"{roomInfo.Name} {roomInfo.PlayerCount} / {maxPlayerCount}";
            roomButton.visible = true;
            roomButton.RegisterCallback<ClickEvent>(e =>{
                UISoundManager.Instance.PlayClickAudio();

                NetworkManager.Instance.JoinRoom(roomInfo);
            });
        };
        //기존에 있는 버튼 삭제
        _roomBtnContainer.Clear();

        //Debug.LogError($"Count: {roomList.Count}");
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
        Debug.Log(player.NickName);
        VisualElement temp = _playerName.Instantiate();

        Label name = temp.Q<Label>("PlayerName");
        _playerMenuContainer.Add(name);
        name.text = player.NickName;
        for(int i = 0; i< _playerMenuContainer.childCount; i++) {
            Debug.Log(_playerMenuContainer[i].name);
        }
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
    public void SetRoomNameText(string roomName) {
        string result = $"{roomName}";
        _roomNameLabel.text = result;
    }
}