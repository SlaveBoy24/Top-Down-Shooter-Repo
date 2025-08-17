using System.Collections;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    public static MainPlayer Instance;

    public GameObject LocalPlayerObject;

    public PlayerStats Stats;
    public PlayerPerks Perks;
    public GlobalInventory Inventory;


    public bool Initialize()
    {
        Instance = this;

        Stats.Initialize();
        Perks.Initialize();
        Inventory.Initialize();

        StartCoroutine(WaitLocalPlayerForInit());

        return true;
    }

    private IEnumerator WaitLocalPlayerForInit()
    {
        yield return new WaitUntil(() => MainPlayer.Instance.LocalPlayerObject != null);

        LocalPlayerObject.GetComponentInChildren<PlayerCameraRenderer>().Initialize();
    }
}
