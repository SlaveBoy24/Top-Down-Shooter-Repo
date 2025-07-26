using UnityEngine;
using Photon.Pun;

public class GamePlayer : MonoBehaviour
{
    [SerializeField] private bool _isLocalPlayer;
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private PhotonView _playerPhotonView;

    public void Start()
    {
        _isLocalPlayer = _playerPhotonView.Owner == PhotonNetwork.LocalPlayer;

        if (!_isLocalPlayer)
            _playerObject.tag = "Untagged";

        _playerObject.SetActive(true);
    }

    public bool IsLocal()
    { 
        return _isLocalPlayer;
    }
}
