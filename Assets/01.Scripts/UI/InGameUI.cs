using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class InGameUI : MonoBehaviour {
    [SerializeField] private TMP_Text _lastPlayerCount;
    [SerializeField] private CameraNameUI _cameraNameUI;
    [SerializeField] private KillLogUI _killLogUI;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private ScorePanelUI _scorePanel;
    [SerializeField] private ProvocationData _provocationData;
    [SerializeField] private Transform _killLogUIParent;
    [SerializeField] private OptionPanelUI _optionPanelUI;
    [SerializeField] private Transform _informationPanelParent;
    [SerializeField] private InformationPanel _informationPanel;
    [SerializeField] private EndPanelUI _endPanelUI;

    private TMP_Text _loadingText;
    public static InGameUI Instance;
    private PhotonView _PV;
    private void Awake() {
        Instance = this;
        _PV = GetComponent<PhotonView>();
        _loadingText  = _loadingPanel.transform.Find("LoadingText").GetComponent<TMP_Text>();
        _loadingPanel.gameObject.SetActive(true);
        _scorePanel.gameObject.SetActive(false);
        _optionPanelUI.ContinueGame();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (_optionPanelUI.Enabled) {
                _optionPanelUI.ContinueGame();
            }
            else {
                _optionPanelUI.OpenPanel();
            }
        }
    }
    public void EndGameUI(){
        _endPanelUI.ShowingSequence();
    }
    public void OnNextRound() {
        RoomManager.Instance.InitGame();
    }
    public void RpcMethod(int count) {
        _PV.RPC("SetLastPlayerText", RpcTarget.All, count);
    }
    [PunRPC]
    public void SetLastPlayerText(int count) {
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
        _loadingPanel.gameObject.SetActive(false);
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

    public void SetPlayerNameUI(string nickName,bool result) {
        _cameraNameUI.gameObject.SetActive(result);
        if (result) {
            _cameraNameUI.SetPlayerText(nickName);
        }
    }
    public void GameEnd(){
        _PV.RPC("GameEndRPC",RpcTarget.All);
    }

    [PunRPC]
    public void GameEndRPC(){
        _scorePanel.ShowingSequence();
        string provocation = _provocationData.RandomProvocation();
        _scorePanel.SetProvocationText(provocation);

        int killCount = (int)RoomManager.Instance.GetPlayerData().killCount;
        int score = (int)RoomManager.Instance.GetPlayerData().score;
        int rank = (int)RoomManager.Instance.GetPlayerData().currentRank;
        int maxPlayer= (int)RoomManager.Instance.GetPlayerData().maxPlayer;

        _scorePanel.SetScoreText((int)killCount,(int)score);        
        _scorePanel.SetRankText((int)rank,(int)maxPlayer);
        _scorePanel.gameObject.SetActive(true);
    }
}