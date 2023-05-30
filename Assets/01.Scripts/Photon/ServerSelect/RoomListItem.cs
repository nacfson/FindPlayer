using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour{
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private TMP_Text _playerCountText;
    private Button _button;
    public RoomInfo info;

    public void SetUp(RoomInfo info,int maxPlayerCount,bool result = true){
        this.info = info;
        _button = GetComponent<Button>();
        _roomNameText.SetText(info.Name);
        _button.enabled = result;
        _playerCountText.text = $"{info.PlayerCount} / {maxPlayerCount}";
    }
    public void OnClick(){
        NetworkManager.Instance.JoinRoom(info);
    }
}