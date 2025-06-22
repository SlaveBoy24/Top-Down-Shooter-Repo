using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    public Player Player;
    public LobbyPlayerUI PlayerUI;

    public GameObject PlayerObject;
    public Transform PlayerPosition;
    public bool IsReady;
    public bool IsHost;

    public void SetPlayer(Player player, GameObject prefab, bool isReady, bool isHost)
    {
        Player = player;
        PlayerObject = Instantiate(prefab, PlayerPosition.position, PlayerPosition.rotation);
        IsHost = isHost;
        IsReady = isReady;

        SetUI();
    }

    public void SetPlayer(Player player, GameObject prefab, bool inRoom)
    {
        Player = player;
        PlayerObject = Instantiate(prefab, PlayerPosition.position, PlayerPosition.rotation, transform);

        SetUI(false);
    }

    public void SetUI(bool hasButtons=true)
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            string playerState = "not ready";
            if (IsReady)
                playerState = "ready";
            if (Player != null)
                PlayerUI.SetNickname(Player.NickName);
            PlayerUI.SetStatus(playerState);
            if (hasButtons)
                PlayerUI.SetButtons(Player);

            PlayerUI.gameObject.SetActive(true);
        }
    }

    public void Clear()
    {
        PlayerUI.gameObject.SetActive(false);
        Player = null;
        IsReady = false;
        IsHost = false;

        Destroy(PlayerObject);
    }
}