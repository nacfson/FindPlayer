using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks{
    [SerializeField] private TMP_Text _text;
    private Player _player;
    public void SetUp(Player player){
        this._player = player;
        _text.SetText(_player.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        if(_player == otherPlayer){
            Destroy(this.gameObject);
        }
    }

    public override void OnLeftRoom(){
        Destroy(this.gameObject);
    }
}