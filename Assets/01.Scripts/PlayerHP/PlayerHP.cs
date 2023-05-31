using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHP : AgentHP {
    protected override void DeadProcess() {
        if (_PV.IsMine) {
            Player player = PhotonNetwork.LocalPlayer;
            RoomManager.Instance.DeadPlayer(player, false);
            base.DeadProcess();

            RoomManager.Instance.UpdateState(GameState.SPECTACTOR);
            RoomManager.Instance.RemoveCamera(_cmCam);
        }
    }
}