using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField username;

    [SerializeField] private Button multiplayerButton;

    [SerializeField] private TextMeshProUGUI multiplayerButtonText;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            StartCoroutine(DisconnectPlayer());
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (username.text.Length >= 1)
        {
            multiplayerButton.interactable = true;
        }
        else
        {
            multiplayerButton.interactable = false;
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Multiplayer()
    {
        PhotonNetwork.NickName = username.text;
        multiplayerButtonText.text = "Connecting...";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("Unir-mos a un Lobby");
        SceneManager.LoadScene("MultiplayerScene");
    }
        
    public void Quit()
    {
        #if UNITY_EDITOR
                if(EditorApplication.isPlaying) 
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
        #else
                Application.Quit();
        #endif
    }
    
    IEnumerator DisconnectPlayer()
    {
        //PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }
    }
}
