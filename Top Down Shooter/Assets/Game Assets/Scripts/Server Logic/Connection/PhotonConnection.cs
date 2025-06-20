using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public static PhotonConnection Instance;
    [SerializeField] private bool _connectedToServer;
    [SerializeField] private bool _connectedToLobby;
    public GameObject LoadingPanelTest;
    public GameObject NicknamePanelTest;
    public TMP_InputField NicknameInputTest;

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

    public void SetNickname(string nickname)
    { 
        PhotonNetwork.NickName = nickname;
        PlayerPrefs.SetString("Nickname", nickname);
        //NicknamePanelTest.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    private void ConnectToServer()
    {
        Debug.Log("Connecting");

        //NetworkClient.Instance;

        LoadingPanelTest.SetActive(true);

        PhotonNetwork.ConnectUsingSettings();

        return;
        if (PlayerPrefs.HasKey("PlayerID"))
        {
            //PhotonNetwork.NickName = PlayerPrefs.GetString("Nickname");
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            NicknamePanelTest.SetActive(true);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.UserId);
        base.OnConnectedToMaster();
        _connectedToServer = true;
        Debug.Log("Connected to Server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        _connectedToLobby = true;
        LoadingPanelTest.SetActive(false);
        Debug.Log("Connected to Lobby");
    }

    public bool IsConnected()
    {
        return (_connectedToServer && _connectedToLobby);
    }

}
