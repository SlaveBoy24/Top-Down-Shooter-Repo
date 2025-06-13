using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public static PhotonConnection Instance;
    [SerializeField] private bool _connectedToServer;
    [SerializeField] private bool _connectedToLobby;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            ConnectToServer();
        }
        else
            Destroy(gameObject);
    }

    private void ConnectToServer()
    {
        Debug.Log("Connecting");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        _connectedToServer = true;
        Debug.Log("Connected to Server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        _connectedToLobby = true;
        Debug.Log("Connected to Lobby");
    }

    public bool IsConnected()
    {
        return (_connectedToServer && _connectedToLobby);
    }

}
