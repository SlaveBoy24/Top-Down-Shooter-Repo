using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Security.Cryptography;
using System.Text;
using System;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public GameObject testLobbyPanel;

    [SerializeField] private bool _isInRoom;
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

    public void CreateRoom()
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

        PhotonNetwork.CreateRoom(shortUniqueId, roomOptions, null);
    }

    public void JoinRoom()
    {
        /*PhotonNetwork.JoinRoom(_findRoomInput.text);

        _findRoomPanel.SetActive(false);*/
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

        _isInRoom = false;

        /*testCreateRoomBtn.SetActive(true);
        testRoomPanel.SetActive(false);*/
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        _isInRoom = true;

        /*testCreateRoomBtn.SetActive(false);
        testRoomPanel.SetActive(true);*/

        Room currentRoom = PhotonNetwork.CurrentRoom;
        _roomManager.SetRoom(currentRoom);
        Player localPlayer = PhotonNetwork.LocalPlayer;

        ExitGames.Client.Photon.Hashtable roomCustomProperties = currentRoom.CustomProperties;

        roomCustomProperties.Add(localPlayer.NickName, localPlayer.IsMasterClient);

        currentRoom.SetCustomProperties(roomCustomProperties);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        _roomManager.UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Room currentRoom = PhotonNetwork.CurrentRoom;
        ExitGames.Client.Photon.Hashtable roomCustomProperties = currentRoom.CustomProperties;
        roomCustomProperties.Remove(otherPlayer.NickName);
        currentRoom.SetCustomProperties(roomCustomProperties);
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
