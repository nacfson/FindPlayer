using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class InGameUI : MonoBehaviour {
    [SerializeField] private TMP_Text _lastPlayerCount;
    [SerializeField] private CameraNameUI _cameraNameUI;
    public static InGameUI Instance;
    private void Awake() {
        Instance = this;
    }
    public void SetLastPlayerText(int count) {
        _lastPlayerCount.SetText(count.ToString());
    }

    public void SetPlayerNameUI(Player player,bool result) {
        _cameraNameUI.enabled = result;
        if (result) {
            _cameraNameUI.SetPlayerText(player);
        }
    }
} 