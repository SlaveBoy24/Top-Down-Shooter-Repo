using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;
using Photon.Realtime;
using TMPro;

public class FriendController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _prefabFriendProfile;

    [Header("UI Panels")]
    [SerializeField] private GameObject _friendsPanel;
    [SerializeField] private GameObject _requestPanel;
    [SerializeField] private GameObject _findFriendsPanel;

    [Header("UI Containers")]
    [SerializeField] private Transform _friendsContainer;
    [SerializeField] private Transform _requestContainer;
    [SerializeField] private Transform _findFriendsContainer;

    [Header("UI Instances Lists")]
    [SerializeField] private List<FriendInfoPanel> _friendsInfoPanels;
    [SerializeField] private List<FriendInfoPanel> _requestInfoPanels;
    [SerializeField] private List<FriendInfoPanel> _findFriendsInfoPanels;

    [Header("Logic Lists")]
    [SerializeField] private Friends _friends = new Friends();
    [SerializeField] private Friends _request = new Friends();
    [SerializeField] private Friends _findFriends = new Friends();


    [Header("Photon")]
    [SerializeField] private PhotonRoom _photonRoom;

    private void Start()
    {
        StartCoroutine(Initialization());
    }

    private IEnumerator Initialization()
    {
        yield return new WaitUntil(() => NetworkClient.Instance.IsInitialized());

        NetworkClient.Instance.GetSocket().Emit("get_friend_list");
        NetworkClient.Instance.GetSocket().Emit("get_request_friend_list");
    }

    public void ShowPanel(string value)
    {
        _friendsPanel.SetActive(false);
        _requestPanel.SetActive(false);
        _findFriendsPanel.SetActive(false);

        switch (value)
        {
            case "friends":
                _friendsPanel.SetActive(true);
                break;

            case "requests":
                _requestPanel.SetActive(true);
                break;

            case "findFriends":
                _findFriendsPanel.SetActive(true);
                break;
        }
    }

    public void UpdateFriendStatus(PingFromFriend pingFromFriend)
    {
        foreach (FriendInfoPanel friend in _friendsInfoPanels)
        {
            if (friend.GetUsername() == pingFromFriend.username)
            {
                friend.SetStatus(pingFromFriend.status);
            }
        }
    }

    #region Setups
    public void SetupFriendList(Friends value)
    {
        _friends.list = value.list;

        ClearList(_friendsInfoPanels);

        foreach (Friend friend in _friends.list)
        {
            AddElementToList(friend, _friendsInfoPanels, _friendsContainer, 1);
        }
    }

    public void SetupRequestList(Friends value)
    {
        _request.list = value.list;

        ClearList(_requestInfoPanels);

        foreach (Friend friend in _request.list)
        {
            AddElementToList(friend, _requestInfoPanels, _requestContainer, 0);
        }
    }

    public void SetupFindFriendsList(Friends value)
    {
        _findFriends.list = value.list;

        ClearList(_findFriendsInfoPanels);

        foreach (Friend friend in _findFriends.list)
        {
            AddElementToList(friend, _findFriendsInfoPanels, _findFriendsContainer, 2);
        }
    }
    #endregion

    public void ClearList(List<FriendInfoPanel> list)
    {
        foreach (var item in list)
        {
            Destroy(item.gameObject);
        }

        list.Clear();
    }

    public void AddElementToList(Friend friend, List<FriendInfoPanel> list, Transform container, int idInfoPanel)
    {
        FriendInfoPanel friendInfoPanel = Instantiate(_prefabFriendProfile, container).GetComponent<FriendInfoPanel>();
        friendInfoPanel.SetInfoPanel(friend, this, idInfoPanel);

        list.Add(friendInfoPanel);
    }

    public void InviteFriend(Friend friend)
    {
        if (_photonRoom == null)
            _photonRoom = FindFirstObjectByType<PhotonRoom>();

        if (PhotonNetwork.CurrentRoom == null)
            _photonRoom.CreateRoom(() => Invite(friend));
        else
            Invite(friend);
    }

    private void Invite(Friend friend)
    {
        Invite invite = new Invite(friend, PhotonNetwork.CurrentRoom.Name);

        NetworkClient.Instance.GetSocket()
            .Emit("invite_friend", new JSONObject(JsonUtility.ToJson(invite)));
    }

    public void AddFriend(Friend friend)
    {
        NetworkClient.Instance.GetSocket()
            .Emit("add_friend", new JSONObject(JsonUtility.ToJson(friend)));
    }

    public void AcceptFriendRequest(Friend friend)
    {
        NetworkClient.Instance.GetSocket()
            .Emit("accept_friend_request", new JSONObject(JsonUtility.ToJson(friend)));
    }

    public void DeclineFriendRequest(Friend friend)
    {
        NetworkClient.Instance.GetSocket()
            .Emit("decline_friend_request", new JSONObject(JsonUtility.ToJson(friend)));
    }

    public void SearchFriend(TMP_InputField tMP_InputField)
    {
        SearchFriend searchFriend = new SearchFriend();
        searchFriend.username = tMP_InputField.text;

        NetworkClient.Instance.GetSocket()
            .Emit("search_friend", new JSONObject(JsonUtility.ToJson(searchFriend)));
    }
}
