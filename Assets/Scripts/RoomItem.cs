using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    public TextMeshProUGUI roomName;

    private NetworkingManager _manager;
    // Start is called before the first frame update
    void Start()
    {
        _manager = FindObjectOfType<NetworkingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoomName(string newRoomName)
    {
        roomName.text = newRoomName;
    }

    public void OnClickItem()
    {
        int index = roomName.text.IndexOf(" - ");
        string room = roomName.text.Substring(0,index);
        _manager.JoinRoom(room);
    }
}
