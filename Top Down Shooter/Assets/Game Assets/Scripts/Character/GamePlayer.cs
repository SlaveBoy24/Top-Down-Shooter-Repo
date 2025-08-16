using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GamePlayer : MonoBehaviour
{
    [SerializeField] private bool _isLocalPlayer;
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private PhotonView _playerPhotonView;
    [SerializeField] private ClothSystem _clothSystem;

    public void Start()
    {
        _isLocalPlayer = _playerPhotonView.IsMine;

        if (!_isLocalPlayer)
        {
            _playerObject.tag = "Untagged";
            UpdatePlayer(_playerPhotonView.Owner);
        }
        else
            MainPlayer.Instance.LocalPlayerObject = gameObject;

        _playerObject.SetActive(true);
    }

    public void SetActivePlayer(bool value)
    {
        _playerObject.SetActive(value);
    }

    public void UpdatePlayer(Player player)
    {
        if (player == PhotonNetwork.LocalPlayer)
            return;

        if (player != _playerPhotonView.Owner)
            return;

        _clothSystem.OnUpdatePlayerProperties(player);
    }

    public bool IsLocal()
    { 
        return _isLocalPlayer;
    }
}
