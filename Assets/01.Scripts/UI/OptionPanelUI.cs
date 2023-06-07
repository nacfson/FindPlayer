using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelUI : MonoBehaviour{
    private List<Button> _buttonList = new List<Button>();
    private bool _enabled = false;
    public bool Enabled => _enabled;
    void Awake() {
        transform.Find("Grid").GetComponentsInChildren<Button>(_buttonList);
    }
    public void QuitGame() {
        Application.Quit();
    }
    public void OpenPanel() {
        gameObject.SetActive(true);
        _enabled = true;
        RoomManager.Instance.UpdateState(GAME_STATE.UI,false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ContinueGame() {
        gameObject.SetActive(false);
        _enabled = false;
        RoomManager.Instance.UpdateState(GAME_STATE.INGAME, false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


}