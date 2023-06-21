using UnityEngine;
using TMPro;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using DG.Tweening;
using UnityEngine.UIElements;
using System;
using UnityEditor;
using System.Collections.Generic;

public class ChatSystem : MonoBehaviour, IChatClientListener{
    public ChatClient chatClient;

    private TextField __inputField;
    private VisualElement __main;
    private Label __stateLabel;
    private VisualElement __root;
    private VisualElement __parentVisual;

    public List<string> messageList = new List<string>();

    [SerializeField] private UIDocument _chatUI;
    [SerializeField] private VisualTreeAsset __chatLabel;
    private bool _isOn;
    private bool _isFocus = false;

    private ScrollView __chatView;
    private bool _isSequence = false;


    private void OnEnable() {
        Application.runInBackground = true;
        chatClient = new ChatClient(this);
        chatClient.ChatRegion = "asia"; // Chat Region을 설정합니다. 예: "asia" 또는 "us"
        chatClient.Connect("5886f062-dc46-4f6f-a050-b5352ae8a786", "2.42", new AuthenticationValues(PhotonNetwork.NickName));
        
        _isSequence = false;

        __root = _chatUI.rootVisualElement;
        
        __parentVisual = __root.Q<VisualElement>("VisualElement");
        __chatView = __root.Q<ScrollView>("ChatView");
        Button sendButton = __root.Q<Button>("SendButton");
        sendButton.RegisterCallback<ClickEvent>(e=>{
            SendMessage();
        });
        __inputField = __root.Q<TextField>("TextField");
        __stateLabel = __root.Q<Label>("StateLabel");
        gameObject.SetActive(true);
        _isOn = false;

        __inputField.RegisterCallback<FocusInEvent>(OnFocusIn);
        __inputField.RegisterCallback<FocusOutEvent>(OnFocusOut);
    }

    private void OnFocusOut(FocusOutEvent evt){
        _isFocus = false;
    }

    private void OnFocusIn(FocusInEvent evt){
        _isFocus = true;
    }

    private void Update(){
        chatClient.Service();
            if(_isOn){
                if(Input.GetKeyDown(KeyCode.Return)){
                                    SendMessage();
                    __inputField.ElementAt(0).Focus();
                }
                if(messageList.Count > 0){
                    foreach(string a in messageList){
                        CreateText(a);
                    }
                    messageList.Clear();
                }
            }
        
        

    }
    public void ShowingSequence(){
        __parentVisual.AddToClassList("active");
        _isOn = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

    }
    public void UnShowingSequence(){
        __parentVisual.RemoveFromClassList("active");
        _isOn = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    public void DebugReturn(DebugLevel level, string message){
        Debug.Log($"Chat Debug - Level: {level}, Message: {message}");
    }

    public void OnChatStateChange(ChatState state){
        Debug.Log($"Chat State Changed - State: {state}");
    }

    public void OnConnected(){
        Debug.Log("Connected to Chat Server");
        __stateLabel.text = "Connected to Chat Server\n";
        SubscribeToChatRoom(PhotonNetwork.CurrentRoom.Name);
    }   
    public void SubscribeToChatRoom(string channelName){
        chatClient.Subscribe(new string[] { channelName });
    }
    public void OnDisconnected(){
        Debug.Log("Disconnected from Chat Server");
        __stateLabel.text = "Disconnected from Chat Server\n";
    }


    public void OnGetMessages(string channelName, string[] senders, object[] messages){
        for (int i = 0; i < senders.Length; i++){
            string sender = senders[i];
            string message = messages[i].ToString();
            //Debug.LogError($"[{channelName}] {sender}: {message}");

            //messageList.Add($"{sender}: {message}");

            messageList.Add($"{sender}: {message}");
            //chatText.text += $"[{channelName}] {sender}: {message}\n";
        }
    }

    public void CreateText(string result){
        VisualElement temp = __chatLabel.Instantiate();
        Label text = temp.Q<Label>("ChatLabel");
        __chatView.Add(text);
        text.text = ($"{result}");
        __chatView.ScrollTo(text);
    }

    public void OnPrivateMessage(string sender, object message, string channelName){
        Debug.Log($"Private Message - Sender: {sender}, Message: {message}, Channel: {channelName}");
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message){
        Debug.Log($"Status Update - User: {user}, Status: {status}, Got Message: {gotMessage}, Message: {message}");
    }

    public void OnSubscribed(string[] channels, bool[] results){
        Debug.Log("Subscribed to Channels:");
        foreach (string channel in channels){
            Debug.Log(channel);
        }
    }

    public void OnUnsubscribed(string[] channels){
        Debug.Log("Unsubscribed from Channels:");
        foreach (string channel in channels){
            Debug.Log(channel);
        }
    }

    public void OnUserSubscribed(string channel, string user){
        Debug.Log($"User Subscribed - Channel: {channel}, User: {user}");
    }

    public void OnUserUnsubscribed(string channel, string user){
        Debug.Log($"User Unsubscribed - Channel: {channel}, User: {user}");
    }

    public void SendMessage(){
        Debug.Log("SendMessage");
        string message = __inputField.text;
        if(message.Trim() != string.Empty){
            chatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, message);
        }
        __inputField.value = string.Empty;
    }

    public bool IsActive(){
        return _isOn;
    }
    public bool IsFocus(){
        return _isFocus;
    }
}
