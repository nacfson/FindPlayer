using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class RoomListItem : MonoBehaviour{
    [SerializeField] private TMP_Text _text;
    public RoomInfo info;
    public void SetUp(RoomInfo info){
        this.info = info;
        _text.SetText(info.Name);
    }
    public void OnClick(){
        NetworkManager.Instance.JoinRoom(info);
    }
}