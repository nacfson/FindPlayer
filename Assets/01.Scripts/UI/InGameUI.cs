using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class InGameUI : MonoBehaviour {
    [SerializeField] private TMP_Text _lastPlayerCount;
    [SerializeField] private CameraNameUI _cameraNameUI;
    [SerializeField] private KillLogUI _killLogUI;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private GameObject _uiCam;
    private TMP_Text _loadingText;
    public static InGameUI Instance;
    private PhotonView _PV;
    private void Awake() {
        Instance = this;
        _PV = GetComponent<PhotonView>();
        _loadingText  = _loadingPanel.transform.Find("LoadingText").GetComponent<TMP_Text>();
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
        _uiCam.gameObject.SetActive(false);
    }

    
    public void CreateKillLogUI(Player killer,Player deader) {
        _PV.RPC("CreateKillLogUIRPC", RpcTarget.All, killer, deader);
    }
    [PunRPC]
    public void CreateKillLogUIRPC(Player killer,Player deader) {
        KillLogUI kui = Instantiate<KillLogUI>(_killLogUI, transform.position, Quaternion.identity);
        kui.transform.position = transform.position;
        kui.SetUI(killer, deader);
        kui.transform.SetParent(this.transform);
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
        
    }
} 