using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class InGameUI : MonoBehaviour {
    [SerializeField] private TMP_Text _lastPlayerCount;
    [SerializeField] private CameraNameUI _cameraNameUI;
    public static InGameUI Instance;
    private PhotonView _PV;
    private void Awake() {
        Instance = this;
        _PV = GetComponent<PhotonView>();
    }

    public void RpcMethod(int count) {
        _PV.RPC("SetLastPlayerText", RpcTarget.All, count);
    }
    [PunRPC]
    public void SetLastPlayerText(int count) {
            _lastPlayerCount.SetText(count.ToString());
    }

    public void SetPlayerNameUI(string nickName,bool result) {
        _cameraNameUI.gameObject.SetActive(result);
        if (result) {
            _cameraNameUI.SetPlayerText(nickName);
        }
    }
} 