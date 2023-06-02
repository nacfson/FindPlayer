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
        //죽은 사람만 카메라를 바꿀 수 있도록 코드를 수정해야함.
        Debug.LogError(cameraIndex);
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