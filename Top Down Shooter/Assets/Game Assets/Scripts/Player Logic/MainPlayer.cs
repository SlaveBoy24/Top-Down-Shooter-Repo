using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    public static MainPlayer Instance;

    public PlayerStats Stats;
    public PlayerPerks Perks;
    public GlobalInventory Inventory;


    public bool Initialize()
    {
        Instance = this;

        Stats.Initialize();
        Perks.Initialize();
        //Inventory.Initialize(); | temporary commented

        return true;
    }
}
