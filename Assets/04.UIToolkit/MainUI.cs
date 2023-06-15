using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour {
    private UIDocument _uiDocument;
    private TextField _nameField;

    private void Awake() {
        _uiDocument = GetComponent<UIDocument>();   
    }
    private void OnEnable() {
        VisualElement root = _uiDocument.rootVisualElement;

        Button popupBtn = root.Q<Button>("StartBtn");
        VisualElement titleMenu = root.Q<VisualElement>("TitleMenu");
        VisualElement roomMenu = root.Q<VisualElement>("RoomMenu");
        _nameField = root.Q<TextField>("NameField");

        popupBtn.RegisterCallback<ClickEvent>(e => {
            //메뉴 로드
            //RoomManager.Instance.InitGame();
            titleMenu.RemoveFromClassList("active");
            roomMenu.AddToClassList("active");
            NetworkManager.Instance.JoinLobby();
        });
    }

    public string GetText() {
        return _nameField.text;
    }

}