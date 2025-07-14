using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using System.Collections;

public class LobbyPlayer : MonoBehaviour
{
    public Player Player;
    public LobbyPlayerUI PlayerUI;
    public GameObject InventoryPanelTest;
    public GameObject LoadingPanelTest;
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

        StartCoroutine(SetupLocal(player));

        SetUI();
    }

    public void SetPlayer(Player player, GameObject prefab, bool inRoom)
    {
        Player = player;
        PlayerObject = Instantiate(prefab, PlayerPosition.position, PlayerPosition.rotation, transform);

        StartCoroutine(SetupLocal(player));

        SetUI(false);
    }

    private IEnumerator SetupLocal(Player player)
    {
        if (player == PhotonNetwork.LocalPlayer)
        {
            RoomManager.LocalPlayerObject = PlayerObject;
            InventoryPanelTest.SetActive(true);
            yield return new WaitForSeconds(.1f);
            InventoryPanelTest.SetActive(false);
            LoadingPanelTest.SetActive(false);
        }
        yield return null;
    }

    public void SetUI(bool hasButtons = true)
    {
        if (PhotonNetwork.CurrentRoom != null && Player != null)
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