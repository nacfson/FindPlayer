using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;

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
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","Player"),Vector3.zero,Quaternion.identity);
    }
}