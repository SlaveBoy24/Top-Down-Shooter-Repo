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
    [SerializeField] private GameObject _acceptButton;
    [SerializeField] private GameObject _declineButton;
    [SerializeField] private GameObject _addFriendButton;

    public string GetUsername()
    {
        return _friend.username;
    }
    public void SetStatus(string status)
    {
        _isOnline = CheckStatus(status);

        SetUI(1);
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
    public void SetInfoPanel(Friend friend, FriendController friendController, int idInfoPanel)
    {
        _friendController = friendController;
        _friend = friend;

        _isOnline = CheckStatus(_friend.status);

        SetUI(idInfoPanel);
    }

    public void InviteFriend()
    {
        _friendController.InviteFriend(_friend);
    }

    public void AcceptFriendRequest()
    {
        _friendController.AcceptFriendRequest(_friend);

        Destroy(gameObject);
    }

    public void DeclineFriendRequest()
    {
        _friendController.DeclineFriendRequest(_friend);

        Destroy(gameObject);
    }

    public void AddFriend()
    {
        _friendController.AddFriend(_friend);
    }

    private void SetUI(int idInfoPanel)
    {
        switch (idInfoPanel)
        {
            case 0:
                _acceptButton.SetActive(true);
                _declineButton.SetActive(true);
                _inviteButton.SetActive(false);
                _addFriendButton.SetActive(false);

                _nickname.text = $"{_friend.username}";
                break;
            case 1:
                _acceptButton.SetActive(false);
                _declineButton.SetActive(false);
                _addFriendButton.SetActive(false);

                string lampColour = _isOnline ? "green" : "red";
                _nickname.text = $"{_friend.username} <color={lampColour}>●</color>";

                if (_isOnline)
                    _inviteButton.SetActive(true);
                else
                    _inviteButton.SetActive(false);
                break;
            case 2:
                _acceptButton.SetActive(false);
                _declineButton.SetActive(false);
                _inviteButton.SetActive(false);
                _addFriendButton.SetActive(true);

                _nickname.text = $"{_friend.username}";
                break;
        }

    }

}
