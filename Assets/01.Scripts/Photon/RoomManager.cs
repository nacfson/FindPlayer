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
public enum GameState { 
    LOBBY = 0, INROOM = 1,INGAME = 2,SPECTACTOR = 3
}

public class RoomManager : MonoBehaviourPunCallbacks{
    public static RoomManager Instance;
    public Dictionary<Player,bool> playerDictionary = new Dictionary<Player,bool>();
    public Dictionary<Player,CinemachineVirtualCamera> cameras = new Dictionary<Player,CinemachineVirtualCamera>();
    public GameState CurrentState => _currentState;
    private GameState _currentState;
    private PhotonView _PV;
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

        InGameUI.Instance.SetLastPlayerText(ReturnPlayerCount());
    }
    public void UpdateState(GameState state) {
        if (_PV.IsMine) {
            this._currentState = state;
        }
    }
    public override void OnEnable(){
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void OnDisable(){
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void ChangeCamera(){
        CinemachineVirtualCamera currentCamera = cameras.Values.ElementAt(_cameraIndex);

        _cameraIndex = (_cameraIndex + 1) % cameras.Count;
        Player targetPlayer = null;
        foreach(var c in cameras){
            if(c.Value == currentCamera) {
                targetPlayer = c.Key;
            }
            c.Value.enabled = false;
        }
        currentCamera.enabled = true;
        InGameUI.Instance.SetPlayerNameUI(targetPlayer,true);
    }
    public void AddCamera(Player player,CinemachineVirtualCamera cmCam) {
        if (_PV.IsMine) {
            cameras.Add(player,cmCam);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){
        if(scene.buildIndex == Define.GameSceneIndex){
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerManager"),Vector3.zero, Quaternion.identity);

            List<Player> playerList = PhotonNetwork.PlayerList.ToList();
            InitPlayer(playerList);
            UpdateState(GameState.INGAME);
        }
    }

    public void DeadPlayer(Player player,bool result) {
        if (playerDictionary.ContainsKey(player)) {
            playerDictionary[player] = result;
            if(InGameUI.Instance != null) {
                InGameUI.Instance.SetLastPlayerText(ReturnPlayerCount());

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