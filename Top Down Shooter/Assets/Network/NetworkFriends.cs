using System;
using TMPro;
using UnityEngine;

public class NetworkFriends : MonoBehaviour
{
    [SerializeField] private GameObject _prefabFriendProfile;
    [SerializeField] private GameObject _friendsPanel;

    [SerializeField] private Friends _friends = new Friends();

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
    }

    public void InitFriendsInfo()
    {
        NetworkClient.Instance.GetSocket()
            .Emit("get_friend_list");
    }

    
}
