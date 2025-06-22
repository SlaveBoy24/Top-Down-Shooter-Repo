using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DeclineInvitePanel : MonoBehaviour
{
    [SerializeField] private Invite _inviteData;
    public void InitDeclineInvitePanel(Invite invite)
    {
        _inviteData = invite;

        var declineInviteText = GetComponentInChildren<TMP_Text>();
        declineInviteText.text = $"{_inviteData.to_username} declined your invite";
        StartCoroutine(DestroyThis());
    }

    private IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(10f);

        Destroy(gameObject);
    }
}
