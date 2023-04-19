using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private TMP_InputField roomInputField;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private Button createRoom;
    void Start()
    {
        PhotonNetwork.JoinLobby();
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

   
    // Update is called once per frame
    void Update()
    {
        if (roomInputField.text.Length >= 1)
        {
            createRoom.interactable = true;
        }
        else
        {
            createRoom.interactable = false;
        }
    }
    
   
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 6,
            PublishUserId = true
        };

        PhotonNetwork.CreateRoom(roomInputField.text, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
    }

    public void Menu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainMenu");
    }
}
