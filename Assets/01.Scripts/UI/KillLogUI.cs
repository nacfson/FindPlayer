using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class KillLogUI: MonoBehaviour {
    [SerializeField] private TMP_Text _killLogText;

    public void SetUI(Player killer, Player deader) {
        string killerValue = killer.NickName;
        string deaderValue = deader.NickName;

        string result = $"{killerValue} Killed {deaderValue}";
        _killLogText.SetText(result);
    }



}

