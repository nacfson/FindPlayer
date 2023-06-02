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
} 