using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private PhotonRoom _photonRoom;
    [SerializeField] private Room _currentRoom;

    [SerializeField] private bool _isReady;

    [SerializeField] private List<LobbyPlayer> _playerInstances;
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private GameObject _testStartGameBtn;
    [SerializeField] private GameObject _testChangeReadyStateBtn;

    public void Initialize(PhotonRoom photonRoom)
    {
        _photonRoom = photonRoom;

        ResetRoom();
    }

    public void SetRoom(Room room)
    {
        _currentRoom = room;

        foreach (LobbyPlayer lobbyPlayer in _playerInstances)
        {
            lobbyPlayer.Clear();
        }

        bool isHost = PhotonNetwork.LocalPlayer.IsMasterClient;
        bool isReady = (bool)_currentRoom.CustomProperties[PhotonNetwork.LocalPlayer.NickName];
        _playerInstances[0].SetPlayer(PhotonNetwork.LocalPlayer, _playerPrefab, isReady, isHost);

        _testStartGameBtn.SetActive(isHost);
        _testChangeReadyStateBtn.SetActive(!isHost);
    }

    public void ResetRoom()
    {
        _currentRoom = null;
        
        foreach (LobbyPlayer lobbyPlayer in _playerInstances)
        {
            lobbyPlayer.Clear();
        }

        _playerInstances[0].SetPlayer(PhotonNetwork.LocalPlayer, _playerPrefab, true, true);

        _testStartGameBtn.SetActive(true);
        _testChangeReadyStateBtn.SetActive(!false);
    }

    public void UpdatePlayerList()
    {
        int playerCount = _currentRoom.PlayerCount;
        ExitGames.Client.Photon.Hashtable customRoomProperties = _currentRoom.CustomProperties;

        for (int i = 0; i < playerCount; i++)
        {
            Player player = _currentRoom.Players[i+1];
            Debug.Log($"playerCount - {playerCount}; id - {i}; player - {player};");
            if (player == null)
                continue;

            if (!PlayerInList(player))
            {
                bool isReady = (bool)customRoomProperties[player.NickName];
                bool isHost = player.IsMasterClient;
                AddPlayerToList(player, isHost, isReady);
            }
        }

        _isReady = (bool)_currentRoom.CustomProperties[PhotonNetwork.LocalPlayer.NickName];

        UpdateUI();
    }

    public void ChangeLocalReadyState()
    {
        _isReady = !_isReady;
        ExitGames.Client.Photon.Hashtable customRoomProperties = _currentRoom.CustomProperties;
        customRoomProperties[PhotonNetwork.LocalPlayer.NickName] = _isReady;
        _currentRoom.SetCustomProperties(customRoomProperties);
    }


    private bool PlayerInList(Player player)
    {
        for (int i = 0; i < _playerInstances.Count; i++)
        { 
            if (_playerInstances[i].Player.UserId == player.UserId)
                return true;
        }

        return false;
    }
    private void AddPlayerToList(Player player, bool isHost, bool isReady)
    {
        for (int i = 0; i < _playerInstances.Count; i++)
        {
            if (_playerInstances[i].Player == null)
            {
                _playerInstances[i].SetPlayer(player, _playerPrefab, isReady, isHost);
                return;
            }
        }
    }

    public void RemovePlayer(Player player)
    {
        for (int i = 0; i < _playerInstances.Count; i++)
        {
            if (_playerInstances[i].Player.UserId == player.UserId)
            {
                _playerInstances[i].Clear();
            }
        }
    }

    private void UpdateUI()
    {
        foreach (LobbyPlayer player in _playerInstances)
        {
            player.SetUI();
        }
    }

    public void StartGame()
    {
        _photonRoom.StartGame();
    }
}
