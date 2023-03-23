using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    private int randomRoomName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindMatch()
    {
        PhotonNetwork.JoinRandomRoom();
        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        MakeRoom();
    }

    public void MakeRoom()
    {
        
    }
}
