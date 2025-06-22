using System.Collections;
using TMPro;
using UnityEngine;

public class InvitePanel : MonoBehaviour
{
    [SerializeField] private Invite _inviteData;

    public void InitInvitePanel(Invite invite)
    {
        _inviteData = invite;

        var inviteText = GetComponentInChildren<TMP_Text>();
        inviteText.text = $"{_inviteData.where_username} invite you";

        StartCoroutine(DestroyThis());
    }
    private IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(20f);

        NetworkClient.Instance.GetSocket()
            .Emit("invite_decline", new JSONObject(JsonUtility.ToJson(_inviteData)));

        Destroy(gameObject);
    }
    public void Accept()
    {
        FindFirstObjectByType<PhotonRoom>().JoinRoom(_inviteData.photon_room);
        Destroy(gameObject);
    }

    public void Decline()
    {
        NetworkClient.Instance.GetSocket()
            .Emit("invite_decline", new JSONObject(JsonUtility.ToJson(_inviteData)));

        Destroy(gameObject);
    }
}
