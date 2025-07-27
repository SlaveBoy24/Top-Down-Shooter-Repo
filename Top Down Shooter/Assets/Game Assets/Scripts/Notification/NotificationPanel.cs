using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationPanel : MonoBehaviour
{
    public void InitNotificationPanel(string text)
    {
        var declineInviteText = GetComponentInChildren<TMP_Text>();
        declineInviteText.text = text;

        StartCoroutine(DestroyThis());
    }

    private IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(10f);

        Destroy(gameObject);
    }
}
