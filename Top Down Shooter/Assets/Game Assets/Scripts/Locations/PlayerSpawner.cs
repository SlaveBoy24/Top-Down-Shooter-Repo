using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

[System.Serializable]
public class PlayerSpawnPoint
{
    public Transform SpawnPoint;
    public bool IsAwailable = true;
}

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private List<Player> _players;
    [SerializeField] private List<GamePlayer> _playersObjects;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private List<PlayerSpawnPoint> _playerPositions;

    public void Initialize()
    {
        _photonView = PhotonView.Get(this);

        if (!_players.Contains(PhotonNetwork.LocalPlayer))
        {
            int positionIndex = GetAvailableSpawnPoint();

            _photonView.RPC("SpawnPlayer", RpcTarget.All, PhotonNetwork.LocalPlayer, positionIndex);

            GameObject instantiatedPlayer = PhotonNetwork.Instantiate(
                _playerPrefab.name,
                _playerPositions[positionIndex].SpawnPoint.position,
                Quaternion.identity
            );

            _photonView.RPC("AddPlayerObject", RpcTarget.All, instantiatedPlayer.GetPhotonView().ViewID);
        }
    }

    [PunRPC]
    private void SpawnPlayer(Player player, int positionIndex)
    {
        _players.Add(player);
        _playerPositions[positionIndex].IsAwailable = false;
    }

    [PunRPC]
    private void AddPlayerObject(int instantiatedID)
    {
        _playersObjects.Add(PhotonView.Find(instantiatedID).gameObject.GetComponent<GamePlayer>());
    }

    public void UpdatePlayer(Player player)
    {
        foreach (GamePlayer gamePlayer in _playersObjects)
        { 
            gamePlayer.UpdatePlayer(player);
        }
    }

    private int GetAvailableSpawnPoint()
    {
        for (int i = 0; i < _playerPositions.Count; i++)
        { 
            if (_playerPositions[i].IsAwailable)
                return i;
        }

        return -1;
    }
}
