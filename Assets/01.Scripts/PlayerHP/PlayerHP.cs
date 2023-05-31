using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHP : AgentHP {
    protected override void DeadProcess() {
        Debug.LogError($"IsMine {_PV.IsMine}");
        if (_PV.IsMine) {
            Player player = PhotonNetwork.LocalPlayer;
            RoomManager.Instance.DeadPlayer(player, false);
            _actionData.IsDead = true;
            base.DeadProcess();
        }
    }
}