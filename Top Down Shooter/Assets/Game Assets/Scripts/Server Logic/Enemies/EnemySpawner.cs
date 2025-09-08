using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private GameObject _enemyPrefab;

    public void Initialize()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Transform spawnPoint in _spawnPoints)
            {
                PhotonNetwork.Instantiate(_enemyPrefab.name, spawnPoint.position, Quaternion.identity);
            }
        }
    }
}
