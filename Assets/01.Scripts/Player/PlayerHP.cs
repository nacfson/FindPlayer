using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHP : AgentHP {
    protected override void DeadProcess(Player attacker) {
        if(RoomManager.Instance.playerDictionary[attacker] == false) return;

        RoomManager.Instance.DeadPlayer(attacker,_agentCamera,false);
        _agentController.MethodRpc(); 
        IsDead = true;
    }
}