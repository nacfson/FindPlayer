using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;
using Photon.Realtime;
using DG.Tweening;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class InGameUI : MonoBehaviour {
    [SerializeField] private KillLogUI _killLogUI;
    [SerializeField] private CameraNameUI _cameraNameUI;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private EndPanelUI _endPanelUI;
    [SerializeField] private ScorePanelUI _scorePanel;
    [SerializeField] private OptionPanelUI _optionPanelUI;
    [SerializeField] private ProvocationData _provocationData;
    [SerializeField] private ProvocationData _winProvocationData;
    [SerializeField] private InformationPanel _informationPanel;
    [SerializeField] private Transform _informationPanelParent;
    [SerializeField] private Transform _killLogUIParent;
    [SerializeField] private ClockUI _clockUI;
    [SerializeField] private ChatSystem _chatSystem;

    [SerializeField] private TMP_Text _lastPlayerCount;
    private TMP_Text _loadingText;

    public event Action OnClockEnd;
    public static InGameUI Instance;
    private PhotonView _PV;
    
    private void Awake() {
        Instance = this;
        _PV = GetComponent<PhotonView>();
        //_loadingText  = _loadingPanel.transform.Find("LoadingText").GetComponent<TMP_Text>();
        //_loadingPanel.gameObject.SetActive(true);
        //_scorePanel.gameObject.SetActive(false);
        _optionPanelUI.ContinueGame();
    }
    private void Update() {
        GAME_STATE currentState = RoomManager.Instance.CurrentState;
        if(currentState == GAME_STATE.LOADING) return;
        if(_chatSystem.IsFocus()) return;
        if(Input.GetKeyDown(KeyCode.T)){
            if(_chatSystem.IsActive()){
                ShowingSequence(false);
            }
            else{
                ShowingSequence(true);
            }
        }
    }
    public void ShowingSequence(bool result){
        if(result){
            _chatSystem.ShowingSequence();
            RoomManager.Instance.UpdateState(GAME_STATE.CHAT,false);
        }
        else{
            _chatSystem.UnShowingSequence();
            RoomManager.Instance.UpdateState(GAME_STATE.INGAME,false);
        }

    }
    public void ShowChat(){
        _chatSystem.ShowingSequence();
    }
    public void EndGameUI(float delay = 0f){
        _endPanelUI.ShowingSequence(delay);
    }

    public void OnNextRound(float delay = 0f) {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);
        sequence.AppendCallback(() => RoomManager.Instance.InitGame());
    }
    public void SetLastPlayerText(int count) {
        _PV.RPC("SetLastPlayerTextRPC", RpcTarget.All, count);
    }
    [PunRPC]
    public void SetLastPlayerTextRPC(int count) {
        _lastPlayerCount.SetText(count.ToString());
    }
    public void SetLoadingText(string value){
        _PV.RPC("SetLoadingTextRPC",RpcTarget.All,value);
    }
    public void GameStart(){
        _PV.RPC("GameStartRPC",RpcTarget.All);
    }
    
    [PunRPC]
    public void SetLoadingTextRPC(string value){
        _loadingPanel.gameObject.SetActive(true);
        _loadingText.SetText(value);
    }

    [PunRPC]
    public void GameStartRPC(){
        //_loadingPanel.gameObject.SetActive(false);
        InformationPanel ifp = Instantiate<InformationPanel>(_informationPanel,_informationPanelParent);
        ifp.ShowingSequence("가짜 중에서 진짜 플레이어를\n 찾아내 살아남아라!",3f);
    }

    public void CreateKillLogUI(Player killer,Player deader) {
        _PV.RPC("CreateKillLogUIRPC", RpcTarget.All, killer, deader);
    }

    [PunRPC]
    public void CreateKillLogUIRPC(Player killer,Player deader) {
        KillLogUI kui = Instantiate<KillLogUI>(_killLogUI, transform.position, Quaternion.identity);
        kui.transform.SetParent(_killLogUIParent);
        kui.SetUI(killer, deader);
        kui.ShowingSequence();
    }
    public void SetClockUI(float currentValue, float maxValue,bool result = true) {
        if(result == false) {
            OnClockEnd?.Invoke();
        }
        _clockUI.SetUI(currentValue, maxValue,result);
    }

    public void GameEnd(Player player,bool gameEnd = false){
        _PV.RPC("GameEndRPC",RpcTarget.All,player,gameEnd);
    }

    [PunRPC]
    public void GameEndRPC(Player player,bool gameEnd = false){
        _scorePanel.ShowingSequence();
        Player localPlayer = PhotonNetwork.LocalPlayer;

        string provocation = _provocationData.RandomProvocation();
        if(localPlayer == player){ 
            provocation = _winProvocationData.RandomProvocation();
        }
        _scorePanel.SetProvocationText(provocation);

        int killCount = (int)RoomManager.Instance.GetPlayerData().killCount;
        int score = (int)RoomManager.Instance.GetPlayerData().score;
        int rank = (int)RoomManager.Instance.GetPlayerData().currentRank;
        int maxPlayer= (int)RoomManager.Instance.GetPlayerData().maxPlayer;

        _scorePanel.SetScoreText((int)killCount,(int)score);        
        _scorePanel.SetRankText((int)rank,(int)maxPlayer);
        _scorePanel.gameObject.SetActive(true);

        if(gameEnd == false){
            OnNextRound(10f);
        }
    }
}