using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PhotonFriends : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LobbyInitialization());
    }

    private IEnumerator LobbyInitialization()
    {
        yield return new WaitUntil(() => IsConnectedToServer());

        StartCoroutine(UpdateFriendsStatus());
    }

    private IEnumerator UpdateFriendsStatus()
    {
        yield return new WaitForSeconds(5);

        /*PhotonNetwork.FindFriends(new List<string>());*/

        StartCoroutine(UpdateFriendsStatus());
    }


    private bool IsConnectedToServer()
    {
        if (PhotonConnection.Instance)
            return PhotonConnection.Instance.IsConnected();
        else
            return false;
    }

}
