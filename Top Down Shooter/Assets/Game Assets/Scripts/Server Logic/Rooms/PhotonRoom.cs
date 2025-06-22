using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine.Events;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public GameObject testLobbyPanel;

    public Action ActionsToExecuteOnRoomJoin;

    [SerializeField] private List<string> _playersNicknames;
    [SerializeField] private RoomManager _roomManager;

    private void Start()
    {
        StartCoroutine(LobbyInitialization());
    }

    private IEnumerator LobbyInitialization()
    {
        yield return new WaitUntil(() => IsConnectedToServer());

        _roomManager = FindAnyObjectByType<RoomManager>();
        _roomManager.Initialize(this);

        testLobbyPanel.SetActive(true);
    }

    private bool IsConnectedToServer()
    {
        if (PhotonConnection.Instance)
            return PhotonConnection.Instance.IsConnected();
        else
            return false;
    }

    public void CreateRoom(Action action = null)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.PublishUserId = true;
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = false;

        string shortUniqueId = "";

        using (SHA256 sha256 = SHA256.Create())
        {
            string uniqueString = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + Guid.NewGuid().ToString();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(uniqueString));

            // Convert hash to a shorter Base64 string
            shortUniqueId = Convert.ToBase64String(hashBytes)
                                    .TrimEnd('=')
                                    .Replace('/', '_')
                                    .Replace('+', '-')
                                    .Substring(0, 8); // Adjust length as needed
        }

        if (action != null)
            ActionsToExecuteOnRoomJoin += action;

        PhotonNetwork.CreateRoom(shortUniqueId, roomOptions, null);
    }


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void JoinRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.Log("бля не получилось");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        CreateRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        _roomManager.ResetRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        ActionsToExecuteOnRoomJoin?.Invoke();

        Room currentRoom = PhotonNetwork.CurrentRoom;
        Player localPlayer = PhotonNetwork.LocalPlayer;

        ExitGames.Client.Photon.Hashtable roomCustomProperties = currentRoom.CustomProperties;

        if (!roomCustomProperties.ContainsKey(localPlayer.NickName))
            roomCustomProperties.Add(localPlayer.NickName, localPlayer.IsMasterClient);
        else
            roomCustomProperties[localPlayer.NickName] = localPlayer.IsMasterClient;

        currentRoom.SetCustomProperties(roomCustomProperties);
        _roomManager.SetRoom(currentRoom);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Room currentRoom = PhotonNetwork.CurrentRoom;
        ExitGames.Client.Photon.Hashtable roomCustomProperties = currentRoom.CustomProperties;
        if (roomCustomProperties.ContainsKey(otherPlayer.NickName));
            roomCustomProperties.Remove(otherPlayer.NickName);
        currentRoom.SetCustomProperties(roomCustomProperties);

        _roomManager.RemovePlayer(otherPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        _roomManager.UpdatePlayerList();
    }

    public void StartGame()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("Game");
    }
}
