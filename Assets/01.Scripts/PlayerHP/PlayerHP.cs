using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHP : AgentHP {
    protected override void DeadProcess() {
        Player player = PhotonNetwork.LocalPlayer;
        RoomManager.Instance.DeadPlayer(player,false);
        _agentController.MethodRpc();
    }
}