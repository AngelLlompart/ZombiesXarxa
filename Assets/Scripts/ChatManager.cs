using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    private ChatClient _chatClient;
    [SerializeField] private TMP_InputField chatText;
    [SerializeField] private TextMeshProUGUI chat;
    private GameManager _gameManager;
    
    private bool chatActive = false;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _chatClient = new ChatClient(this);
        _chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName));
    }

    // Update is called once per frame
    void Update()
    {
        _chatClient.Service();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            chatText.DeactivateInputField();
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _gameManager.paused = false;
            SendMessage();
        }
        
        if (Input.GetKeyDown(KeyCode.T) && chatActive == false)
        {
            _gameManager.paused = true;
            chatText.Select();
            chatText.ActivateInputField();
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
       // throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        _chatClient.Subscribe(new string[] { "RegionChannel" });
    }

    public void OnChatStateChange(ChatState state)
    {
        //throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";
        for (int i = 0; i < senders.Length; i++)
        {
            msgs = String.Format("{0}: {1}", senders[i], messages[i]);
            chat.text += "\n " + msgs;
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
       // throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
       // throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
      //  throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
       // throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
      //  throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
       // throw new System.NotImplementedException();
    }

    public void SendMessage()
    {
        if (chatText.text != "")
        {
            _chatClient.PublishMessage("RegionChannel", chatText.text);
        }
        chatText.text = "";
        _gameManager.UnPause();
        chatText.DeactivateInputField();
    }
    
}
