using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class CameraManager : MonoBehaviour {
    private List<AgentCamera> _agentCameraList = new List<AgentCamera>();
    public static CameraManager Instance;
    public PhotonView photonView;

    private void Awake() {
        Instance = this;
        photonView = GetComponent<PhotonView>();
    }
    public void AddCamera(AgentCamera agentCamera) {
        _agentCameraList.Add(agentCamera);
    }
    public void ChangeCamera(int cameraIndex) {
        //죽은 사람만 카메라를 바꿀 수 있도록 코드를 수정해야함.
        Debug.LogError(cameraIndex);
        AgentCamera currentCamera = _agentCameraList[cameraIndex];
    

        foreach(var a in _agentCameraList){
            CinemachineVirtualCamera camera = a.GetCamera();
            if(a == currentCamera){
                camera.enabled = true;
            }
            else{
                camera.enabled = false;
            }
        }
        InGameUI.Instance.SetPlayerNameUI(currentCamera.GetPlayer().NickName, true);
    }
    public void RemoveCamera(AgentCamera agentCamera) {
        _agentCameraList.Remove(agentCamera);
    }
    public int GetCameraCount(){
        return _agentCameraList.Count;
    }
}