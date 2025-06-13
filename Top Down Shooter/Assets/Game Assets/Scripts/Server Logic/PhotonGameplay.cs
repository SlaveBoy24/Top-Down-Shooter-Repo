using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonGameplay : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private List<GameObject> _players;
    [SerializeField] private Transform _spawnPoint;

    private void Start()
    {
        _players.Add(PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoint.position, Quaternion.identity));
    }
}
