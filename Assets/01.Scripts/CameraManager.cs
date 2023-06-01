using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class CameraManager : MonoBehaviour {
    public Dictionary<string, CinemachineVirtualCamera> cameraDictionary = new Dictionary<string, CinemachineVirtualCamera>();
    public static CameraManager Instance;
    public PhotonView photonView;

    private void Awake() {
        Instance = this;
        photonView = GetComponent<PhotonView>();
    }
    private int _cameraIndex = 0;

    public void AddCamera(string playerName, CinemachineVirtualCamera camera) {
        if (!cameraDictionary.ContainsKey(playerName)) {
            cameraDictionary.Add(playerName, camera);
        }
        else {
            // �̹� �ش� �÷��̾��� ī�޶� �߰��Ǿ� �ִ� ���, ���� ī�޶� ��ü�մϴ�.
            cameraDictionary[playerName] = camera;
        }
    }
    public void ChangeCamera(int cameraIndex) {
        photonView.RPC("ChangeCameraRPC", RpcTarget.All,cameraIndex);
    }

    [PunRPC]
    public void ChangeCameraRPC(int cameraIndex) {
        if (photonView.IsMine == false) return;

        CinemachineVirtualCamera currentCamera = cameraDictionary.Values.ElementAt(cameraIndex);

        string targetPlayer = null;
        foreach (var c in cameraDictionary) {
            if (c.Value == currentCamera) {
                targetPlayer = c.Key;
            }
            c.Value.enabled = false;
        }
        currentCamera.enabled = true;
        InGameUI.Instance.SetPlayerNameUI(targetPlayer, true);
    }
    public void RemoveCamera(string playerName) {
        if (cameraDictionary.ContainsKey(playerName)) {
            cameraDictionary.Remove(playerName);
        }
    }
}