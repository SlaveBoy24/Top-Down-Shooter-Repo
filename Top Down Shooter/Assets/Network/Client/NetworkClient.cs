using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using Project;
using Project.Utility;
using Photon.Realtime;

public class NetworkClient : SocketIOComponent
{
    public static NetworkClient Instance;
    [SerializeField] private bool _isInitialized;
    private SocketIOComponent _socket;
    [SerializeField] private string _ip = "127.0.0.1";
    [SerializeField] private int _port = 80;
    [HideInInspector]public NetworkIdentity NetworkIdentity;
    [SerializeField] private FriendController _friendController;

    [SerializeField] private GameObject _notificationZone;

    [SerializeField] private GameObject _testInvitePrefab;
    [SerializeField] private GameObject _testDeclineInvitePrefab;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);
        
        SetAddress(_ip, _port);
        Debug.Log($"Setup Address - {_ip}:{_port}");

        base.Awake();
    }

    public override void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        NetworkIdentity = Instance.GetComponent<NetworkIdentity>();

        base.Start();
        ConnectClient();

        SetSocketReference(this);
    }

    public void ConnectClient()
    {
        SetupEvents();
        SetupEventsForFriendSystem();

        base.Connect();
    }
    private void SetupEventsForFriendSystem()
    {
        On("friend_list", (E) =>
        {
            Friends friends_data = new Friends();
            friends_data = JsonUtility.FromJson<Friends>(E.data.ToString());

            if (_friendController == null)
                _friendController = FindFirstObjectByType<FriendController>();

            _friendController.SetupFriendList(friends_data);
        });

        On("ping_from_friend", (E) =>
        {
            PingFromFriend ping_from_friend_data = new PingFromFriend();
            ping_from_friend_data = JsonUtility.FromJson<PingFromFriend>(E.data.ToString());

            if (_friendController == null)
                _friendController = FindFirstObjectByType<FriendController>();

            _friendController.UpdateFriendStatus(ping_from_friend_data);
        });

        On("invite_friend", (E) =>
        {
            var newInvite = JsonUtility.FromJson<Invite>(E.data.ToString());

            Debug.Log($"invite from {newInvite.where_username}");

            InvitePanel invite = Instantiate(_testInvitePrefab).GetComponent<InvitePanel>();
            invite.InitInvitePanel(newInvite);
        });

        On("invite_decline", (E) =>
        {
            var newInvite = JsonUtility.FromJson<Invite>(E.data.ToString());

            Debug.Log($"decline invite from {newInvite.to_username}");

            DeclineInvitePanel invite = Instantiate(_testDeclineInvitePrefab, _notificationZone.transform).GetComponent<DeclineInvitePanel>();
            invite.InitDeclineInvitePanel(newInvite);
        });
    }

    private void SetupEvents()
    {
        On("open", (E) =>
        {
            Debug.Log("Connected made to the Server");
        });

        On("init", (E) =>
        {
            PlayerData data = new PlayerData();
            data = JsonUtility.FromJson<PlayerData>(E.data.ToString());

            Debug.Log("Player ID: " + data.id);

            NetworkIdentity.Player.id = data.id;

            if (PlayerPrefs.HasKey("PlayerID"))
            {
                int player_id = PlayerPrefs.GetInt("PlayerID");
                NetworkIdentity.Player.player_id = player_id;
                data.player_id = player_id;
            }

            _socket.Emit("init_confirmed", new JSONObject(JsonUtility.ToJson(data)));
        });

        On("username_init", (E) =>
        {
            PlayerData data = new PlayerData();
            data = JsonUtility.FromJson<PlayerData>(E.data.ToString());

            Debug.Log($"Player ID: {data.player_id}, Player Username: {data.username}");

            if (!PlayerPrefs.HasKey("PlayerID"))
            {
                PlayerPrefs.SetInt("PlayerID", data.player_id);
            }

            if (data.mail != "")
            {
                NetworkIdentity.Player.mail = data.mail;
            }

            if (data.username == "")
            {
                NetworkIdentity.ShowUsernamePanel(true);
                Debug.Log("SetUsername()");
            }
            else
            {
                NetworkIdentity.Player.username = data.username;
                NetworkIdentity.ShowUsernamePanel(false);

                NetworkIdentity.InitInformation();

                _isInitialized = true;
                
                PhotonConnection.Instance.ConnectToServer();
            }

            _socket.Emit("past_init", new JSONObject(JsonUtility.ToJson(NetworkIdentity.Player)));
        });

        On("close", (E) =>
        {
            Debug.Log("Disconnected Client");
            //base.Close(); //Заглушить реконект

        });
    }

    public SocketIOComponent GetSocket()
    {
        return _socket;
    }

    public void SetSocketReference(SocketIOComponent Socket)
    {
        this._socket = Socket;
    }

    public bool IsInitialized()
    {
        return _isInitialized;
    }

}

// GetSocket().Emit("rpc", new JSONObject(JsonUtility.ToJson(data))); // Послать TCP
// GetSocketUDP().Emit("rpc", JsonUtility.ToJson(data).ToString()); //Послать UDP