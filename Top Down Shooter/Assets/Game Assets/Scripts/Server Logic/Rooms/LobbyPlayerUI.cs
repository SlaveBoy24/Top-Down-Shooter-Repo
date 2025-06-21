using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyPlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nicknameText;
    [SerializeField] private TextMeshProUGUI _statusText;

    [SerializeField] private GameObject _kickButton;
    [SerializeField] private GameObject _leaveButton;

    public void SetNickname(string nickname)
    {
        _nicknameText.text = nickname;
    }
    
    public void SetStatus(string status)
    {
        _statusText.text = status;
    }

    public void SetButtons(Player player)
    {
        if (player.NickName == PhotonNetwork.NickName)
        {
            _leaveButton.SetActive(true);
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
                _kickButton.SetActive(true);
        }
    }
}
