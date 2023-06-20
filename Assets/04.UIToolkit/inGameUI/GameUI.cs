using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEditor;
using DG.Tweening;
public class GameUI : MonoBehaviour {
    private UIDocument _uiDoucment;
    [SerializeField] private VisualTreeAsset _scoreMenu;
    [SerializeField] private VisualTreeAsset _killLogUI;


    private PhotonView _PV;
    public static GameUI Instance;

    private bool _isOptionMenu = false;
    private VisualElement _inGameMenu;
    private VisualElement _loadingMenu;
    private VisualElement _optionMenu;
    private VisualElement _optionPanel;
    private VisualElement _scoreBoardMenu;
    private ScrollView _scoreMenuContainer;
    private ScrollView _killLogMenuContainer;

    private VisualElement _selectMenu;
    private VisualElement _selectInMenu;
    private Label _lastPeopleLabel;
    private Label _loadingLabel;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        _uiDoucment = GetComponent<UIDocument>();
        _PV = GetComponent<PhotonView>();
    }

    private void OnEnable() {
        VisualElement root = _uiDoucment.rootVisualElement;

        _inGameMenu = root.Q<VisualElement>("InGameMenu");
        _loadingMenu = root.Q<VisualElement>("LoadingMenu");
        _optionMenu = root.Q<VisualElement>("OptionMenu");
        _scoreBoardMenu = root.Q<VisualElement>("ScoreBoardMenu");

        _optionPanel = _optionMenu.Q<VisualElement>("OptionPanel");
        _selectMenu = _optionMenu.Q<VisualElement>("SelectMenu");
        _selectInMenu = _selectMenu.Q<VisualElement>("SelectInMenu");
        _scoreMenuContainer = _scoreBoardMenu.Q<ScrollView>("ScoreView");
        _killLogMenuContainer = _inGameMenu.Q<ScrollView>("KillLogView");
        //OptionPanelBtns
        Button continueBtn = _optionPanel.Q<Button>("ContinueBtn");
        Button titleBtn = _optionPanel.Q<Button>("TitleBtn");
        continueBtn.RegisterCallback<ClickEvent>(e => {
            CloseOptionMenus();
            RoomManager.Instance.UpdateState(GAME_STATE.INGAME);
            _isOptionMenu = !_isOptionMenu;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        });
        titleBtn.RegisterCallback<ClickEvent>(e => {
            _selectMenu.AddToClassList("active");
            _optionPanel.RemoveFromClassList("active");
        });


        //SelectMenu Btns
        Button okBtn = _selectMenu.Q<Button>("OKBtn");
        Button noBtn = _selectMenu.Q<Button>("NOBtn");

        okBtn.RegisterCallback<ClickEvent>(e => {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.AutomaticallySyncScene = false;
            Destroy(RoomManager.Instance);
            PhotonNetwork.LoadLevel(0);
        });
        noBtn.RegisterCallback<ClickEvent>(e => {
            _selectMenu.RemoveFromClassList("active");
            RoomManager.Instance.UpdateState(GAME_STATE.UI);
            _optionPanel.AddToClassList("active");
        });
        _lastPeopleLabel = _inGameMenu.Q<VisualElement>("LastPeople").Q<Label>("LastPeopleLabel");
        _loadingLabel = _loadingMenu.Q<Label>("LoadingLabel");


        Button exitToTitleBtn = _scoreBoardMenu.Q<Button>("ExitBtn");
        exitToTitleBtn.RegisterCallback<ClickEvent>(e => {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LoadLevel(0);
        });
    }





    private void CloseOptionMenus() {
        _optionMenu.RemoveFromClassList("active");
        _optionPanel.RemoveFromClassList("active");
        _selectMenu.RemoveFromClassList("active");
    }
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if (_isOptionMenu) {
                CloseOptionMenus();
                RoomManager.Instance.UpdateState(GAME_STATE.INGAME);
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
                _isOptionMenu = !_isOptionMenu;
            }
            else {
                _optionMenu.AddToClassList("active");
                RoomManager.Instance.UpdateState(GAME_STATE.UI);
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;

                _isOptionMenu = !_isOptionMenu;
                _optionPanel.AddToClassList("active");
            }
        }

    }
    public void SetLastPlayerText(string result) {
        _PV.RPC("SetLastPlayerTextRPC", RpcTarget.All, result);
    }
    [PunRPC]
    public void SetLastPlayerTextRPC(string result) {
        string value = $"남은인원 {result}";
        _lastPeopleLabel.text = value;
    }
    public void ActiveScoreBoard(string nickName, int killCount, int score) {
        _PV.RPC("ActiveScoreBoardRPC", RpcTarget.All,nickName,killCount,score);
    }
    [PunRPC]
    public void ActiveScoreBoardRPC(string nickName,int killCount,int score) {
        _scoreBoardMenu.AddToClassList("active");
        //Player[] players = PhotonNetwork.PlayerList;

        //foreach(var p in players) {
        //    ExitGames.Client.Photon.Hashtable property = p.CustomProperties;
        //    int killCount = (int)property["KILLCOUNT"];
        //    int score = (int)property["SCORE"];
        //    CreateScoreBoardMenu(p.NickName,killCount,score);
        //}
        CreateScoreBoardMenu(nickName,killCount,score);
    }
    public void CreateScoreBoardMenu(string nickName, int killCount, int score) {
        VisualElement scoreMenu = _scoreMenu.Instantiate();
        VisualElement scoreBoard = scoreMenu.Q<VisualElement>("ScoreMenu");

        _scoreMenuContainer.Add(scoreBoard);

        Label playerNameLabel = scoreBoard.Q<Label>("NameLabel");
        Label killCountLabel = scoreBoard.Q<Label>("KillCountLabel");
        Label scoreLabel = scoreBoard.Q<Label>("ScoreLabel");

        playerNameLabel.text = nickName;
        killCountLabel.text = killCount.ToString();
        scoreLabel.text = score.ToString();
    }


    public void CreateKillLogUI(string attacker,string deader) {
        _PV.RPC("CreateKillLogUIRPC", RpcTarget.All, attacker, deader);
    }
    
    [PunRPC]
    public void CreateKillLogUIRPC(string attacker,string deader) {
        string result = $"{attacker}가 {deader}를 처치";
        VisualElement temp = _killLogUI.Instantiate();
        VisualElement killLogUI = temp.Q<VisualElement>("KillLogUI");

        Label log = killLogUI.Q<Label>("KillLogLabel");
        log.text = result;

        _killLogMenuContainer.Add(killLogUI);
        killLogUI.AddToClassList("active");

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(3f);
        sequence.AppendCallback(() => {
            _killLogMenuContainer.Remove(killLogUI);
        });
    }

    public void SetLoadingText(string result,bool value) {
        _PV.RPC("SetLoadingTextRPC", RpcTarget.All, result, value);
    }
    [PunRPC]
    public void SetLoadingTextRPC(string result, bool value) {
        Action<bool> action = (value) => {
            if (value) {
                _loadingMenu.AddToClassList("active");
            }
            else {
                _loadingMenu.RemoveFromClassList("active");
            }
        };
        action(value);
        string d = $"{result}";
        _loadingLabel.text = d;
    }
}