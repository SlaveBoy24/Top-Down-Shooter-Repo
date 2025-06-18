using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UDPSocket;
using Project;
using Project.Utility;

public class NetworkClient : SocketIOComponent
{
    public static NetworkClient instance;
    private SocketIOComponent socket;
    private UDPSocketComponent socketUDP;
    [SerializeField] private string ip = "127.0.0.1";
    [SerializeField] private int port = 7777;
    private NetworkWorld networkWorld;
    public void Awake()
    {
        SetAddress(ip, port);

        NetworkClient.instance = this;
        base.Awake();
    }

    public override void Start()
    {
        networkWorld = NetworkWorld.instance;
        socketUDP = gameObject.GetComponent<UDPSocketComponent>();

        base.Start();
        ConnectClient();

        SetSocketReference(this);
        SetSocketReference(socketUDP);
    }

    public void ConnectClient()
    {
        SetupEventsTCP();
        SetupEventsUDP();

        base.Connect();
        ConnectUDP();
    }

    public void ConnectUDP()
    {
        socketUDP.connect(ip, port);
        socketUDP.udpSocketState = UDPSocketComponent.UDPSocketState.CONNECTED;
    }

    public void Update()
    {
        if (socketUDP.udpSocketState != UDPSocketComponent.UDPSocketState.CONNECTED)
        {
            socketUDP.udpSocketState = UDPSocketComponent.UDPSocketState.CONNECTED;
        }

        base.Update();
    }

    private void SetupEventsUDP()
    {        
        socketUDP.On("updatePl", new Action<UDPSocketEvent>(OnUpdateTransform));	
    }

    private void OnUpdateTransform(UDPSocketEvent msg)
    {
           
        PlayerData data = new PlayerData();
		data = JsonUtility.FromJson<PlayerData>(msg.pack[1]); 

        networkWorld.UpdatePlayer(data);
                     
    }

    private void SpawnPlayer(PlayerData data, bool idenity)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);        
        cube.transform.position = NetworkUtility.ToVector3(data.position); 

        NetworkIdentity identity = cube.AddComponent<NetworkIdentity>();
        identity.SetId(data.id);
        identity.SetMine(idenity);
        identity.SetSocketTCP(socket);
        identity.SetSocketUDP(socketUDP);

        NetworkTransform networkTransform = cube.AddComponent<NetworkTransform>();
        
        networkWorld.AddPlayer(data.id, networkTransform);
    }

    private void SetupEventsTCP()
    {
        On("open", (E) =>
        {            
            Debug.Log("Connected made to the Server");            
        });        

        On("spawnPlayer", (E) =>
        {
            networkWorld.ClearWorld();

            PlayerData data = new PlayerData();
            data = JsonUtility.FromJson<PlayerData>(E.data.ToString());

            Debug.Log("Spawn player: " + data.id);
           
            SpawnPlayer(data, true);
            
        });

        On("spawnOther", (E) =>
        {
            PlayerData data = new PlayerData();
            data = JsonUtility.FromJson<PlayerData>(E.data.ToString());

            Debug.Log("Spawn player: " + data.id);

            SpawnPlayer(data, false);

        });

        On("disconnected", (E) =>
        {
            string id = E.data["id"].ToString().RemoveQuotes();
            networkWorld.DeletePlayer(id);
        });

        On("close", (E) =>
        {            
            Debug.Log("Disconnected Client");
            //base.Close(); //Заглушить реконект

        });
    }

    public UDPSocketComponent GetSocketUDP()
    {
        return socketUDP;
    }

    public void SetSocketReference(UDPSocketComponent Socket)
    {
        socketUDP = Socket;
    }

    public SocketIOComponent GetSocket()
    {
        return socket;
    }

    public void SetSocketReference(SocketIOComponent Socket)
    {
        socket = Socket;
    }

    public void DisconnectUDP()
    {
        socketUDP.udpSocketState = UDPSocketComponent.UDPSocketState.DISCONNECTED;
    }
}

// GetSocket().Emit("rpc", new JSONObject(JsonUtility.ToJson(data))); // Послать TCP
// GetSocketUDP().Emit("rpc", JsonUtility.ToJson(data).ToString()); //Послать UDP