using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Security.Cryptography;
using System.Text;
using System;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public GameObject testCreateRoomBtn;
    public GameObject testStartGameBtn;

    [SerializeField] private bool _isInRoom;
    [SerializeField] private string _currentRoomName;
    [SerializeField] private List<TextMeshProUGUI> _playersNicknames;

    private void Start()
    {
        StartCoroutine(LobbyInitialization());
    }

    private IEnumerator LobbyInitialization()
    {
        yield return new WaitUntil(()=>IsConnectedToServer());

        testCreateRoomBtn.SetActive(true);
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

    public void JoinRoom(string name)
    { 
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        testCreateRoomBtn.SetActive(false);
        testStartGameBtn.SetActive(true);

        Room currentRoom = PhotonNetwork.CurrentRoom;

        Debug.Log($"i am {PhotonNetwork.NickName} - {PhotonNetwork.LocalPlayer.UserId}");
        Debug.Log($"master's index {currentRoom.masterClientId}");
        Debug.Log($"master is {currentRoom.Players[currentRoom.masterClientId].NickName} - {currentRoom.Players[currentRoom.masterClientId].UserId}");

        Debug.Log($"Connected to Room {currentRoom.Name}");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
    }

    public void StartGame()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("Game");
    }
}
