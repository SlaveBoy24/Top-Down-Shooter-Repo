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
    private NetworkIdentity _networkIdentity;
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
        _networkIdentity = Instance.GetComponent<NetworkIdentity>();

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

            _networkIdentity.id = data.id;
            data.mail = _networkIdentity.mail;

            _socket.Emit("init_confirmed", new JSONObject(JsonUtility.ToJson(data)));
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