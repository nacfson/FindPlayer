using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
public class CameraNameUI : MonoBehaviour {
    [SerializeField] private TMP_Text _playerNameText;
    public void SetPlayerText(string nickName) {
        string value = $"PlayerName: {nickName}";
        _playerNameText.SetText(value);
    }
}