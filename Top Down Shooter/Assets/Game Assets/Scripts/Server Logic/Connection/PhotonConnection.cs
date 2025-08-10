using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;


public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public static PhotonConnection Instance;
    [SerializeField] private bool _connectedToServer;
    [SerializeField] private bool _connectedToLobby;
    [SerializeField] private GlobalInventory _inventory;
    [SerializeField] private GameObject _loadingPanelTest;
    public Action ActionsToExecuteOnJoinedLobby;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        _loadingPanelTest.SetActive(true);
    }

    public void ConnectToServer()
    {
        Debug.Log("Connecting");

        PhotonNetwork.NickName = NetworkClient.Instance.NetworkIdentity.Player.username;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.UserId);
        base.OnConnectedToMaster();
        _connectedToServer = true;
        Debug.Log("Connected to Server");

        _inventory.Initialize();
        _loadingPanelTest.SetActive(false);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        _connectedToLobby = true;
        Debug.Log("Connected to Lobby");

        ActionsToExecuteOnJoinedLobby?.Invoke();
        ActionsToExecuteOnJoinedLobby = null;
    }

    public bool IsConnected()
    {
        return (_connectedToServer && _connectedToLobby);
    }

}
