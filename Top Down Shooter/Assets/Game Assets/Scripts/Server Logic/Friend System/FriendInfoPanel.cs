using UnityEngine;
using TMPro;

public class FriendInfoPanel : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private FriendController _friendController;
    [SerializeField] private Friend _friend;
    [SerializeField] private bool _isOnline;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _nickname;
    [SerializeField] private GameObject _inviteButton;
    public string GetUsername()
    {
        return _friend.username;
    }
    public void SetStatus(string status)
    {
        _isOnline = CheckStatus(status);

        SetUI();
    }
    public bool CheckStatus(string status)
    {
        switch (status)
        {
            case "online":
                return true;
            default:
                return false;
        }
    }
    public void SetInfoPanel(Friend friend, FriendController friendController)
    {
        _friendController = friendController;
        _friend = friend;

        _isOnline = CheckStatus(_friend.status);

        SetUI();
    }

    public void InviteFriend()
    {
        _friendController.InviteFriend(_friend);
    }

    private void SetUI()
    { 
        string lampColour = _isOnline ? "green" : "red";
        _nickname.text = $"{_friend.username} <color={lampColour}>●</color>";

        if (_isOnline)
            _inviteButton.SetActive(true);
        else
            _inviteButton.SetActive(false);
    }

}
