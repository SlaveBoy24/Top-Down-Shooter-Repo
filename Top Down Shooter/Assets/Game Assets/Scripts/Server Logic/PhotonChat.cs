using ExitGames.Client.Photon;
using Photon.Chat;
using AuthenticationValues = Photon.Chat.AuthenticationValues;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class PhotonChat : MonoBehaviour, IChatClientListener
{
    [SerializeField] private bool _initialized;
    [SerializeField] private ChatAppSettings _chatAppSettings;
    [SerializeField] private string _chatState;
    public ChatClient _chatClient;

    private void Start()
    {
        StartCoroutine(ChatInitialization());
    }

    private void FixedUpdate()
    {
        if (_chatClient != null)
        {
            _chatClient.Service();
        }
    }

    private IEnumerator ChatInitialization()
    {
        yield return new WaitUntil(() => IsConnectedToServer());

        ConnectToChat();
    }

    private void ConnectToChat()
    {
        Debug.Log("Connecting To Chat");
        _chatClient = new ChatClient(this);
        _chatClient.UseBackgroundWorkerForSending = true;
        _chatClient.AuthValues = new AuthenticationValues(PhotonNetwork.NickName);
        _chatAppSettings = GetChatSettings(PhotonNetwork.PhotonServerSettings.AppSettings);
        _chatClient.ConnectUsingSettings(_chatAppSettings);
    }

    private bool IsConnectedToServer()
    {
        if (PhotonConnection.Instance)
            return PhotonConnection.Instance.IsConnected();
        else
            return false;
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        Debug.Log("Disconnetcted from Chat");
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        Debug.Log("Connected To Chat");

        _initialized = true;

        _chatClient.Subscribe("Region");
    }

    public void OnChatStateChange(ChatState state)
    {
        _chatState = state.ToString();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            if (!results[i])
            {
                Debug.Log($"Trying connect to '{channels[i]}' again...");
                _chatClient.Subscribe(channels[i]);
            }
            else
            {
                Debug.Log($"Successfully connected to channel '{channels[i]}'");
            }
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public ChatAppSettings GetChatSettings(AppSettings appSettings)
    {
        return new ChatAppSettings
        {
            AppIdChat = appSettings.AppIdChat,
            AppVersion = appSettings.AppVersion,
            FixedRegion = appSettings.IsBestRegion ? null : appSettings.FixedRegion,
            NetworkLogging = appSettings.NetworkLogging,
            Protocol = appSettings.Protocol,
            EnableProtocolFallback = appSettings.EnableProtocolFallback,
            Server = appSettings.IsDefaultNameServer ? null : appSettings.Server,
            Port = (ushort)appSettings.Port,
            ProxyServer = appSettings.ProxyServer
            // values not copied from AppSettings class: AuthMode
            // values not needed from AppSettings class: EnableLobbyStatistics 
        };
    }
}
