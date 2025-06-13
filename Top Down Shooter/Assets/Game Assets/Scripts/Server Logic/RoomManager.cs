using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Security.Cryptography;
using System.Text;
using System;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public GameObject testCreateRoomBtn;
    public GameObject testStartGameBtn;

    private void Start()
    {
        StartCoroutine(LobbyInitialization());
    }

    private IEnumerator LobbyInitialization()
    {
        yield return new WaitUntil(()=>PhotonConnection.Instance.IsConnected());

        testCreateRoomBtn.SetActive(true);
    }

    public void CreateRoom()
    {
        base.OnJoinedLobby();

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

        PhotonNetwork.JoinOrCreateRoom(shortUniqueId, roomOptions, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        testCreateRoomBtn.SetActive(false);
        testStartGameBtn.SetActive(true);

        Room currentRoom = PhotonNetwork.CurrentRoom;

        Debug.Log($"Connected to Room {currentRoom.Name}");
    }

    public void StartGame()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("Game");
    }
}
