using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonGameplay : MonoBehaviourPunCallbacks
{
    [SerializeField] private TeammatesPanelManager _teammatePanelManager;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        Debug.Log($"player prop changed - {targetPlayer.NickName}");

        if (targetPlayer == PhotonNetwork.LocalPlayer)
            return;

        Debug.Log($"call panelmanager");
        StartCoroutine(_teammatePanelManager.UpdateTeammatePanel(targetPlayer));
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
    }
}
