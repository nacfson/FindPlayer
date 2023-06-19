using System.Net;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Core;
using System;
using System.Linq;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using static UnityEngine.Rendering.DebugUI;

public enum GAME_STATE{
    MENU =0, LOADING =1, INGAME = 2,UI = 3, END = 4
}

[System.Serializable]
public class PlayerData{
    public int score;
    public int currentRank;
    public int maxPlayer;
    public int killCount;
}

public class RoomManager : MonoBehaviourPunCallbacks{
    public static RoomManager Instance;
    public Dictionary<Player,bool> playerDictionary = new Dictionary<Player,bool>();

    private PhotonView _PV;
    [SerializeField] private float _loadingTime;
    [SerializeField] private int _initAICount = 50;
    [SerializeField] private int _defineRound = 1; 
    [SerializeField] private ItemObjectListSO _itemObjectList;
    [SerializeField] private MainUI _mainUI; 

    public event Action<Player> OnRoundEnd;


    private bool _gameEnd = false;

    private int _roundCount = 0; 
    private int _cameraIndex = 0;
    public int playerCount;
    private PlayerData _playerData = new PlayerData();
    public GAME_STATE CurrentState => _currentState;
    private GAME_STATE _currentState;

    private void Awake() { 
        if(Instance == null){
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        _PV = GetComponent<PhotonView>();
    }
    public override void OnEnable(){
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable(){
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){
        if(scene.buildIndex == Define.GameSceneIndex){
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerManager"),Vector3.zero, Quaternion.identity);

            List<Player> playerList = PhotonNetwork.PlayerList.ToList();
            UpdateState(GAME_STATE.LOADING);
            InitPlayer(playerList);
            LoadingGame();
            MakeAIPlayer();
            _playerData.maxPlayer = playerList.Count;
            playerCount = playerList.Count;
        }
        else if(scene.buildIndex == Define.RoomIndex && !string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.NickName)){
            //MenuManager.Instance.OpenMenu("room");
            //Debug.LogError("GameInit");
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(0.3f);
            sequence.AppendCallback(() => _mainUI.GameInit());
        }
    }
    public void InitPlayer(List<Player> playerList) {
        playerDictionary.Clear();
        foreach (Player player in playerList) {
            playerDictionary.Add(player, true);
        }
    }
    private void LoadingGame(){
        if(PhotonNetwork.IsMasterClient){
            StartCoroutine(LoadGameCor());
        }
    }

    public void InitGame() {
        _PV.RPC("InitGameRPC", RpcTarget.All);
    }

    [PunRPC]
    public void InitGameRPC() {
        PhotonNetwork.LoadLevel(Define.GameSceneIndex);
    }

    public void ReturnToRoom(){
        _PV.RPC("ReturnToRoomRPC",RpcTarget.All);
    }

    [PunRPC]
    public void ReturnToRoomRPC(){
        PhotonNetwork.LoadLevel(Define.RoomIndex);
    } 

    IEnumerator LoadGameCor(){
        PhotonNetwork.CurrentRoom.IsVisible = false;

        float timer = 0f;
        while(timer < _loadingTime){
            timer += Time.deltaTime;
            string value = ((int)(_loadingTime - timer)).ToString();
            //InGameUI.Instance.SetLoadingText(value);
            GameUI.Instance.SetLoadingText(value,true);
            yield return null;
        }
        InGameUI.Instance.GameStart();
        UpdateState(GAME_STATE.INGAME);
        GameUI.Instance.SetLoadingText("3", false);
        CreateItem();
        GameUI.Instance.SetLastPlayerText(ReturnPlayerCount().ToString());
    }

    private void MakeAIPlayer(){
        if(PhotonNetwork.IsMasterClient){
            for(int i = 0; i < _initAICount; i++) {
                GameObject brain =  PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","AIPlayer"),Vector3.zero, Quaternion.identity);
                brain.transform.position = GameManager.Instance.RandomWayPoint().ReturnPos();
            }
        }
    }

    public void DeadPlayer(Player attacker, AgentCamera agentCamera,bool result = false) {
        Player player = agentCamera.GetPlayer();

        if (playerDictionary.ContainsKey(player)) {
            int index = CameraManager.Instance.GetCameraIndex(agentCamera);
            _PV.RPC("DeadPlayerRPC",RpcTarget.All,player,result,index);

            if(InGameUI.Instance != null) {
                GameUI.Instance.SetLastPlayerText(ReturnPlayerCount().ToString());

                //InGameUI.Instance.CreateKillLogUI(attacker,player);
                GameUI.Instance.CreateKillLogUI(attacker.NickName, player.NickName);
            }
        }

        if (IfRoundEnd()) {
            RoundEnd();
        }
    }


    [PunRPC]
    public void DeadPlayerRPC(Player player,bool result,int cameraIndex = 1024){
        Player localPlayer = PhotonNetwork.LocalPlayer;
        bool samePlayer = localPlayer == player;
        if (samePlayer) {
            SetPlayerCount(player);
        }
        playerDictionary[player] = result;
        CreateItem();

        if (cameraIndex != 1024) {
            AgentCamera agentCamera = CameraManager.Instance.GetIndexToCamera(cameraIndex);
            CameraManager.Instance.RemoveCamera(agentCamera);
        }
    }

    public void CreateItem(){
        _PV.RPC("CreateItemRPC",RpcTarget.All);
    }

    [PunRPC]
    public void CreateItemRPC(){
        if(_PV.IsMine){
            WayPoint wayPoint = GameManager.Instance.RandomWayPoint();
            Vector3 pos = wayPoint.ReturnPos();

            string itemName = _itemObjectList.GetRandomItem().gameObject.name;
            GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs",itemName),pos,Quaternion.identity);
            obj.transform.position = pos;
            obj.gameObject.SetActive(true);
        }
    }


    public void SetPlayerCount(Player player) {
        _PV.RPC("SetPlayerCountRPC", RpcTarget.All, player);
    }

    [PunRPC]
    public void SetPlayerCountRPC(Player player) {
        Player localPlayer = PhotonNetwork.LocalPlayer;

        if (player == localPlayer) {
            _playerData.currentRank = ReturnPlayerCount();
        }
    }

    private void RoundEnd(){
        UpdateState(GAME_STATE.LOADING);
        _roundCount++;

        Player player = null;

        foreach(var p in playerDictionary) {
            if(p.Value == true) {
                player = p.Key;
            }
        }

        if (player == null) {
            Debug.LogError("Player is Null!!!");
        }
        else {
            OnRoundEnd?.Invoke(player);
            UpdateKillCountAndScore(0, 200,player.NickName);
            SetPlayerCount(player);
        }

        if(_roundCount >= _defineRound) {
            GameEnd();
            InGameUI.Instance.GameEnd(player,true);
        }
        else {
            InGameUI.Instance.GameEnd(player);
        }
    }

    public void GameEnd() {
        _PV.RPC("GameEndRPC",RpcTarget.All);

    }

    [PunRPC]
    public void GameEndRPC() {
        //Hashtable hashtable = new Hashtable();
        //hashtable.Add("KILLCOUNT",_playerData.killCount);
        //hashtable.Add("SCORE",_playerData.score);
        //PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(7f);
        sequence.AppendCallback(() => {
            GameUI.Instance.ActiveScoreBoard(PhotonNetwork.LocalPlayer.NickName, _playerData.killCount, _playerData.score);
        });
        Destroy(RoomManager.Instance.gameObject);
        
        //InGameUI.Instance.EndGameUI(12f);
        _gameEnd = true;
    }


    //플레이어 나갔을 때 어떻게 되는지 확인하기
    //플레이어 오브젝트 사라지는지 카메라는 어떻게 되는지
    public void LeftPlayer(Player lefter) {
        if (playerDictionary.ContainsKey(lefter)) {
            if(_PV != null) {
                _PV.RPC("DeadPlayerRPC", RpcTarget.All, lefter, false,1024);
            }
            if (InGameUI.Instance != null) {
                GameUI.Instance.SetLastPlayerText(ReturnPlayerCount().ToString());

            }
            if (IfRoundEnd()) {
                RoundEnd();
            }
            if(PhotonNetwork.PlayerList.Count() <= 1) {
                GameEnd();
            }
        }
    }

    public int ReturnPlayerCount() {
        int count = 0;
        foreach(var p in playerDictionary.Values) {
            if (p) {
                count++;
            }
        }
        return count;
    }

    public void UpdateKillCountAndScore(int killCount = 0,int score = 0,string nickName = null){
        _PV.RPC("UpdateKillCountAndScoreRPC",RpcTarget.All,killCount,score,nickName);
    }

    [PunRPC]
    public void UpdateKillCountAndScoreRPC(int killCount = 1,int score = 100,string nickName= null){
        Player localPlayer = PhotonNetwork.LocalPlayer;
        if(nickName == localPlayer.NickName) {
            _playerData.killCount += killCount;
            _playerData.score += score;
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer) {
        LeftPlayer(otherPlayer);
    }
    public void UpdateState(GAME_STATE state,bool rpc = true){
        if (rpc == false) {
            _currentState = state;
        }
        else {
            _PV.RPC("UpdateStateRPC", RpcTarget.All, state);
        }
    }
    [PunRPC]
    public void UpdateStateRPC(GAME_STATE state){
        _currentState = state;
    }

    public bool IfRoundEnd(){
        return ReturnPlayerCount() <= 1;
    }
    public PlayerData GetPlayerData(){
        return _playerData;
    }
}