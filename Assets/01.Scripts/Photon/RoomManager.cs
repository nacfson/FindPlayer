using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Core;
using Cinemachine;
using System.Linq;
using System;

public class RoomManager : MonoBehaviourPunCallbacks{
    public static RoomManager Instance;
    public Dictionary<Player,bool> playerDictionary = new Dictionary<Player,bool>();
    private PhotonView _PV;
    [SerializeField] private int _initAICount = 50;
    private int _cameraIndex = 0;
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
            InitPlayer(playerList);
            MakeAIPlayer();
        }
    }

    private void MakeAIPlayer(){
        if(PhotonNetwork.IsMasterClient){
            for(int i = 0; i < _initAICount; i++) {
                GameObject brain =  PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","AIPlayer"),Vector3.zero, Quaternion.identity);
                brain.GetComponent<EnemyController>().EnableNavMesh(false);
                brain.transform.position = GameManager.Instance.RandomWayPoint().ReturnPos();
                brain.GetComponent<EnemyController>().EnableNavMesh(true);
            }
        }
    }

    public void DeadPlayer(Player player,bool result) {
        if (playerDictionary.ContainsKey(player)) {
            playerDictionary[player] = result;
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
        DeadPlayer(otherPlayer,false);
    }

}