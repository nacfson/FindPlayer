using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using Photon.Pun;
public class PlayerNameUI : MonoBehaviour {
    private CinemachineVirtualCamera _followCam;
    private TMP_Text _playerNameText;
    private PhotonView _PV;

    private void Awake() {
        _followCam = GetComponentInChildren<CinemachineVirtualCamera>();
        _playerNameText = transform.Find("PlayerName").GetComponent<TMP_Text>();
        _PV = GetComponent<PhotonView>();
    }
    private void Start() {
        string playerName = PhotonNetwork.LocalPlayer.NickName;
        SetPlayerName(playerName);
    }
    private void Update() {
        _playerNameText.transform.rotation = Quaternion.LookRotation(_followCam.transform.forward);
    }
    public void SetPlayerName(string playerName) {
        _PV.RPC("SetPlayerNameRPC", RpcTarget.All, playerName);
    }
    [PunRPC]
    public void SetPlayerNameRPC(string playerName) {
        if (_PV.IsMine) {
            _playerNameText.SetText(playerName);
        }
    }

    public void ShowText(bool result) {
        _playerNameText.gameObject.SetActive(result);
    }


}