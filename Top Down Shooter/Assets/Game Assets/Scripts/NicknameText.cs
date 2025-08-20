using UnityEngine;
using Photon.Pun;
using TMPro;

public class NicknameText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        _text.text = PhotonNetwork.LocalPlayer.NickName;
    }
}
