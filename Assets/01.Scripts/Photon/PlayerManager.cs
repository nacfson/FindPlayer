using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;
using Cinemachine;
public class PlayerManager : MonoBehaviour{
    private PhotonView _PV;
    private void Awake() {
        _PV = GetComponent<PhotonView>();
    }
    private void Start() {
        if(_PV.IsMine){
            CreateController();
        }
    }
    private void CreateController(){
        Debug.Log("Instantiated Player Controller");
        Vector3 pos = GameManager.Instance.RandomWayPoint().ReturnPos();
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","Player"),pos,Quaternion.identity);
        string name = PhotonNetwork.LocalPlayer.NickName;
        obj.name = name;
        _PV.RPC("SetGameObjectNameRPC",RpcTarget.All,name);
    }

    [PunRPC]
    public void SetGameObjectNameRPC(string playerName){
        gameObject.name = playerName;
    } 
}