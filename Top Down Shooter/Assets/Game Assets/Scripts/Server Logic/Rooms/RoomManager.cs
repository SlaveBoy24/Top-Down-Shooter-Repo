using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class LobbyPlayer
{
    public GameObject PlayerObject;
    public Transform PlayerPosition;
    public bool IsHost;
}

public class RoomManager : MonoBehaviour
{
    [SerializeField] private PhotonRoom _photonRoom;
    [SerializeField] private Room _currentRoom;

    [SerializeField] private bool _isReady;

    [SerializeField] private TextMeshProUGUI _roomId;
    [SerializeField] private List<TextMeshProUGUI> _nicknames;
    [SerializeField] private List<TextMeshProUGUI> _states;
    [SerializeField] private Dictionary<string, bool> _players;

    [SerializeField] private GameObject _testStartGameBtn;
    [SerializeField] private GameObject _testChangeReadyStateBtn;

    public void Initialize(PhotonRoom photonRoom)
    {
        _photonRoom = photonRoom;
    }

    public void SetRoom(Room room)
    {
        _currentRoom = room;
        _players = new Dictionary<string, bool>();
        _roomId.text = $"ID: {_currentRoom.Name}";

        _testStartGameBtn.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient);
        _testChangeReadyStateBtn.SetActive(!PhotonNetwork.LocalPlayer.IsMasterClient);
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
            if (!_players.ContainsKey(player.NickName))
            {
                _players.Add(player.NickName, false); 
            }

            _players[player.NickName] = (bool)customRoomProperties[player.NickName];
        }

        _isReady = _players[PhotonNetwork.LocalPlayer.NickName];

        UpdateUI();
    }

    public void ChangeLocalReadyState()
    {
        _isReady = !_isReady;
        ExitGames.Client.Photon.Hashtable customRoomProperties = _currentRoom.CustomProperties;
        customRoomProperties[PhotonNetwork.LocalPlayer.NickName] = _isReady;
        _currentRoom.SetCustomProperties(customRoomProperties);
    }

    private void UpdateUI()
    {
        int index = 0;

        foreach (KeyValuePair<string, bool> entry in _players)
        {
            string playerState = "not ready";
            if (entry.Value)
                playerState = "ready";

            _nicknames[index].text = entry.Key;
            _states[index].text = playerState;
            index++;
        }

        for (int i = index; i < _nicknames.Count; i++)
        {
            Debug.Log(i);
            _nicknames[i].text = "empty";
            _states[i].text = "";
        }
    }

    public void StartGame()
    {
        _photonRoom.StartGame();
    }
}
