using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GamePlayer : MonoBehaviour
{
    [SerializeField] private bool _isLocalPlayer;
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private PhotonView _playerPhotonView;
    [SerializeField] private ClothSystem _clothSystem;
    [SerializeField] private WeaponSystem _weaponSystem;
    [SerializeField] private InteractorCollider _interactor;

    public void Start()
    {
        _isLocalPlayer = _playerPhotonView.IsMine;

        if (!_isLocalPlayer)
        {
            Destroy(_interactor.gameObject);
            _playerObject.tag = "OtherCharacter";
            UpdatePlayer(_playerPhotonView.Owner);
        }
        else
            MainPlayer.Instance.LocalPlayerObject = gameObject;

        _playerObject.SetActive(true);
    }

    public void GetDamage(int value)
    {
        Player player = _playerPhotonView.Owner;
        _playerPhotonView.RPC("GetDamageRPC", player, player, value);
    }

    [PunRPC]
    public void GetDamageRPC(Player player, int value)
    {
        if (player == PhotonNetwork.LocalPlayer && player == _playerPhotonView.Owner)
            MainPlayer.Instance.Stats.ConsumeHealths(value);
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
        _weaponSystem.OnUpdatePlayerProperties(player);
    }

    public bool IsLocal()
    { 
        return _isLocalPlayer;
    }
}
