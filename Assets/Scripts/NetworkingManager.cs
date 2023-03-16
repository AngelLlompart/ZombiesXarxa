using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] private Button multiplayerButton;
    void Start()
    {
        Debug.Log("Connexió a un servidor");
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Unir-mos a un Lobby");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Estam apunt per jugar!");
        multiplayerButton.interactable = true;
    }
}
