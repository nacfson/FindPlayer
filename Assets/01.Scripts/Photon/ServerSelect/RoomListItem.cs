using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class RoomListItem : MonoBehaviour{
    [SerializeField] private TMP_Text _text;
    RoomInfo _info;
    public void SetUp(RoomInfo info){
        this._info = info;
        _text.SetText(_info.Name);
    }
    public void OnClick(){
        NetworkManager.Instance.JoinRoom(_info);
    }
}