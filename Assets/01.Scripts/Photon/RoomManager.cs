using System.Linq.Expressions;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Core;
using System.Linq;
using System;
using System.Collections;
public enum GAME_STATE{
    MENU =0, LOADING =1, INGAME = 2
}
public class RoomManager : MonoBehaviourPunCallbacks{
    public static RoomManager Instance;
    public Dictionary<Player,bool> playerDictionary = new Dictionary<Player,bool>();
    private PhotonView _PV;
    [SerializeField] private float _loadingTime;
    [SerializeField] private int _initAICount = 50;
    private int _cameraIndex = 0;
    public GAME_STATE CurrentState => _currentState;
    private GAME_STATE _currentState;
    private void Awake() { 
        if(Instance){
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
        _PV = GetComponent<PhotonView>();
    }

    public void InitPlayer(List<Player> playerList) {
        foreach(Player player in playerList) {
            playerDictionary.Add(player, true);
        }

        InGameUI.Instance.RpcMethod(ReturnPlayerCount());
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
        }
    }

    private void LoadingGame(){
        if(PhotonNetwork.IsMasterClient){
            StartCoroutine(LoadGameCor());
        }
    }

    IEnumerator LoadGameCor(){
        float timer = 0f;
        while(timer < _loadingTime){
            timer += Time.deltaTime;
            string value = ((int)(_loadingTime - timer)).ToString();
            InGameUI.Instance.SetLoadingText(value);
            yield return null;
        }
        InGameUI.Instance.GameStart();
        UpdateState(GAME_STATE.INGAME);
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
            playerDictionary[player] = result;
            CameraManager.Instance.RemoveCamera(agentCamera);
            if(InGameUI.Instance != null) {
                InGameUI.Instance.RpcMethod(ReturnPlayerCount());
                InGameUI.Instance.CreateKillLogUI(attacker,player);
            }
            if(GameEnd()){
                
            }
        }
    }

    public void LeftPlayer(Player lefter) {
        if (playerDictionary.ContainsKey(lefter)) {
            playerDictionary[lefter] = false;
            if(InGameUI.Instance != null) {
                InGameUI.Instance.RpcMethod(ReturnPlayerCount());
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

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        LeftPlayer(otherPlayer);
    }

    public void UpdateState(GAME_STATE state){
        _PV.RPC("UpdateStateRPC",RpcTarget.All,state);
    }

    [PunRPC]
    public void UpdateStateRPC(GAME_STATE state){
        _currentState = state;
    }

    public bool GameEnd(){
        return ReturnPlayerCount() <= 1;
    }
}