using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using Photon.Pun;

public class GameUI : MonoBehaviour {
    private UIDocument _uiDoucment;
    private PhotonView _PV;
    public static GameUI Instance;

    private VisualElement _inGameMenu;
    private VisualElement _loadingMenu;
    private Label _lastPeopleLabel;
    private Label _loadingLabel;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        _uiDoucment = GetComponent<UIDocument>();
        _PV = GetComponent<PhotonView>();
    }

    private void OnEnable() {
        VisualElement root = _uiDoucment.rootVisualElement;

        _inGameMenu = root.Q<VisualElement>("InGameMenu");
        _loadingMenu = root.Q<VisualElement>("LoadingMenu");
        _lastPeopleLabel = _inGameMenu.Q<VisualElement>("LastPeople").Q<Label>("LastPeopleLabel");
        _loadingLabel = _loadingMenu.Q<Label>("LoadingLabel");

    }

    public void SetLastPlayerText(string result) {
        string value = $"남은인원 {result}";
        _lastPeopleLabel.text = value;
    }
    public void SetLoadingText(string result,bool value) {
        _PV.RPC("SetLoadingTextRPC", RpcTarget.All, result, value);
    }

    [PunRPC]
    public void SetLoadingTextRPC(string result, bool value) {
        Action<bool> action = (value) => {
            if (value) {
                _loadingMenu.AddToClassList("active");
            }
            else {
                _loadingMenu.RemoveFromClassList("active");
            }
        };
        string d = $"{result}";
        _loadingLabel.text = d;
    }
}