using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class InGameUI : MonoBehaviour {
    [SerializeField] private TMP_Text _lastPlayerCount;
    public static InGameUI Instance;
    private void Awake() {
        Instance = this;
    }
    public void SetLastPlayerText(int count) {
        _lastPlayerCount.SetText(count.ToString());
    }
}