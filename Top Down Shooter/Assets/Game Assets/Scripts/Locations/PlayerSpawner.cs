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
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private List<PlayerSpawnPoint> _playerPositions;

    private void Start()
    {
        _photonView = PhotonView.Get(this);

        if (!_players.Contains(PhotonNetwork.LocalPlayer))
        {
            int positionIndex = GetAvailableSpawnPoint();
            GameObject instantiatedPlayer = PhotonNetwork.Instantiate(
                _playerPrefab.name,
                _playerPositions[positionIndex].SpawnPoint.position,
                Quaternion.identity
            );

            _photonView.RPC("SpawnPlayer", RpcTarget.All, PhotonNetwork.LocalPlayer, positionIndex);
        }
    }

    [PunRPC]
    private void SpawnPlayer(Player player, int positionIndex)
    {
        _players.Add(player);
        _playerPositions[positionIndex].IsAwailable = false;
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
