using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using System.Collections;

public class LobbyPlayer : MonoBehaviour
{
    public Player Player;
    public LobbyPlayerUI PlayerUI;
    public GameObject PlayerObject;
    public ClothSystem PlayerClothSystem;
    public Transform PlayerPosition;
    public bool IsReady;
    public bool IsHost;

    public bool IsMain;

    public void SetPlayer(Player player, GameObject prefab, bool isReady, bool isHost)
    {
        Player = player;

        IsMain = player == PhotonNetwork.LocalPlayer;

        if (IsMain && PlayerObject == null)
            PlayerObject = Instantiate(prefab, PlayerPosition.position, PlayerPosition.rotation);
        else if (!IsMain)
            PlayerObject = Instantiate(prefab, PlayerPosition.position, PlayerPosition.rotation);
        IsHost = isHost;
        IsReady = isReady;

        PlayerClothSystem = PlayerObject.GetComponent<ClothSystem>();

        if (player == PhotonNetwork.LocalPlayer)
            RoomManager.LocalPlayerObject = PlayerObject;
        else
            UpdatePlayer();

        SetUI();
    }

    public void UpdatePlayer()
    {
        PlayerClothSystem.OnUpdatePlayerProperties(Player);
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
        IsReady = false;
        IsHost = false;

        if (!IsMain)
        {
            Player = null;
            Destroy(PlayerObject);
        }
    }
}