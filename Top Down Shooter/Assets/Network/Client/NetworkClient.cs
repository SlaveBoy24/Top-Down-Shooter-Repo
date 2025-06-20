using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UDPSocket;
using Project;
using Project.Utility;
using UnityEditor.ShaderGraph.Serialization;

public class NetworkClient : SocketIOComponent
{
    public static NetworkClient Instance;
    private SocketIOComponent _socket;
    [SerializeField] private string _ip = "127.0.0.1";
    [SerializeField] private int _port = 80;
    [HideInInspector]public NetworkIdentity NetworkIdentity;
    public void Awake()
    {
        SetAddress(_ip, _port);
        Debug.Log($"Setup Address - {_ip}:{_port}");

        NetworkClient.Instance = this;
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
        SetupEventsTCP();

        base.Connect();
    }

    private void SetupEventsTCP()
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
                NetworkIdentity.SetUsernamePanel.SetActive(true);
                Debug.Log("SetUsername()");
            }
            else
            {
                NetworkIdentity.Player.username = data.username;
                NetworkIdentity.SetUsernamePanel.SetActive(false);

                NetworkIdentity.InitInformation();
            }
        });

        /*On("spawnOther", (E) =>
        {
            PlayerData data = new PlayerData();
            data = JsonUtility.FromJson<PlayerData>(E.data.ToString());

            Debug.Log("Spawn player: " + data.id);

            //SpawnPlayer(data, false);

        });

        On("disconnected", (E) =>
        {
            string id = E.data["id"].ToString().RemoveQuotes();
            //networkWorld.DeletePlayer(id);
        });*/

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

}

// GetSocket().Emit("rpc", new JSONObject(JsonUtility.ToJson(data))); // Послать TCP
// GetSocketUDP().Emit("rpc", JsonUtility.ToJson(data).ToString()); //Послать UDP