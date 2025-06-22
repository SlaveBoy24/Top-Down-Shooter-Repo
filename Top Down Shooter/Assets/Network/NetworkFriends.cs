using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NetworkFriends : MonoBehaviour
{
    [SerializeField] private GameObject _prefabFriendProfile;
    [SerializeField] private GameObject _friendsPanel;

    [SerializeField] private Friends _friends = new Friends();

    [SerializeField] private PhotonRoom _photonRoom;

    public void SetupFriendList(Friends value)
    {
        _friends.list = value.list;

        ClearFriendPanel();

        foreach (Friend friend in _friends.list)
        {
            AddFriendProfile(friend);
        }
    }

    public void ClearFriendPanel()
    {
        while (_friendsPanel.transform.childCount > 0) {
            DestroyImmediate(_friendsPanel.transform.GetChild(0).gameObject);
        }
    }

    public void AddFriendProfile(Friend friend)
    {
        var friendGameObject = Instantiate(_prefabFriendProfile, _friendsPanel.transform);

        var label = friendGameObject.GetComponentInChildren<TMP_Text>();

        var status_smile = friend.status == "offline" ? "red" : "green";

        label.text = $"{friend.username} <color={status_smile}>●</color>";

        var button = friendGameObject.GetComponentInChildren<Button>();

        button.onClick.AddListener(() => InviteFriend(friend));
    }

    public void InitFriendsInfo()
    {
        NetworkClient.Instance.GetSocket()
            .Emit("get_friend_list");
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
        friend.roomNameTest = PhotonNetwork.CurrentRoom.Name;
        NetworkClient.Instance.GetSocket()
            .Emit("invite_friend", new JSONObject(JsonUtility.ToJson(friend)));
    }
}
