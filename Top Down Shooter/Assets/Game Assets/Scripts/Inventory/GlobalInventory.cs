using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalInventory : MonoBehaviour
{
    [HideInInspector] public static GlobalInventory Instance;
    public GameObject ItemPrefab;

    [SerializeField] protected Stash Backpack;
    [SerializeField] protected Stash Stash;
    public Stash ArmourHeadSlot;
    public Stash ArmourChestSlot;
    public Stash ArmourLegsSlot;
    public Stash MainWeaponSlot;
    public Stash SecondaryWeaponSlot;
    public Stash BackpackSlot;

    private bool _initialized;

    public void Initialize()
    {
        if (_initialized)
            return;

        Instance = this;

        GlobalStashes.Backpack = Backpack;
        GlobalStashes.Stash = Stash;

        GlobalStashes.ClearEvents();
        GlobalStashes.OnChangeInventoryEvent += OnChangeInventoryEvent;
        GlobalStashes.OnReadyStashEvent += OnReadyStashEvent;

        BackpackSlot.Initialize();
        ArmourHeadSlot.Initialize();
        ArmourChestSlot.Initialize();
        ArmourLegsSlot.Initialize();
        MainWeaponSlot.Initialize();
        SecondaryWeaponSlot.Initialize();

        Stash.Initialize();

        _initialized = true;
    }

    private void OnReadyStashEvent(GlobalStashes.EventArgs e)
    {
        Debug.Log($"Stash: {e.Stash.transform.gameObject.name} invoke");

        if (!PlayerPrefs.HasKey(e.Stash.transform.gameObject.name))
        {
            if (e.Stash.transform.gameObject.name == "Backpack Slot")
                GlobalStashes.Backpack.UpdateSlotCount();

            return;
        }

        string json = PlayerPrefs.GetString(e.Stash.transform.gameObject.name, "Unknown");

        JSONStash.ItemsWrapper Items = json.JSONToItems();

        foreach (JSONStash.ItemWrapper item in Items.i)
        {
            e.Stash.SpawnItemByKeyWithID(ItemPrefab, item.son, item.sid, item.c);
        }

        CheckChangeEquipmentSlot(e);
    }

    private void OnChangeInventoryEvent(GlobalStashes.EventArgs e)
    {
        if (e.PastStash != null)
            PlayerPrefs.SetString(e.PastStash.transform.gameObject.name, e.PastStash.ItemsToJSON());
        PlayerPrefs.SetString(e.Stash.transform.gameObject.name, e.Stash.ItemsToJSON());

        CheckChangeEquipmentSlot(e);
    }

    private void CheckChangeEquipmentSlot(GlobalStashes.EventArgs e)
    {
        List<Stash> list = new List<Stash> { e.Stash };

        if (e.PastStash != null)
            list.Add(e.PastStash);

        foreach (Stash stash in list)
        {
            switch (stash.transform.gameObject.name)
            {
                case "Backpack":
                    continue;
                case "Stash":
                    continue;
                default:
                    StartCoroutine(OnChangeEquipmentEvent(stash));
                    break;
            }
        }
    }

    private IEnumerator OnChangeEquipmentEvent(Stash e)
    {
        switch (e.transform.gameObject.name)
        {
            case "Backpack Slot":
                if (e.Items[0] != null)
                    GlobalStashes.Backpack.UpdateSlotCount(e.Items[0].item.SlotCount);
                else
                    GlobalStashes.Backpack.UpdateSlotCount();

                break;
            default:
                yield return new WaitUntil(() => MainPlayer.Instance.LocalPlayerObject != null);

                var clothSystem = MainPlayer.Instance.LocalPlayerObject.GetComponent<ClothSystem>();

                if (e.Items[0] == null)
                {
                    clothSystem.DeEquipCloth(e.transform.gameObject.name);

                    break;
                }

                clothSystem.EquipCloth(new ClothBase(e));

                break;
        }
    }
}

[Serializable]
public static class GlobalStashes
{
    public class EventArgs
    {
        public EventArgs(Stash stash) { Stash = stash; }
        public EventArgs(Stash stash, Stash past_stash) { Stash = stash; PastStash = past_stash; }
        public Stash Stash { get; }
        public Stash PastStash { get; }
    }
    public delegate void OnChangeInventory(EventArgs e);
    public static event OnChangeInventory OnChangeInventoryEvent;
    public static void Invoke(Stash stash)
    {
        OnChangeInventoryEvent?.Invoke(new EventArgs(stash));
    }
    public static void Invoke(Stash stash, Stash past_stash)
    {
        OnChangeInventoryEvent?.Invoke(new EventArgs(stash, past_stash));
    }
    public delegate void OnReadyStash(EventArgs e);
    public static event OnReadyStash OnReadyStashEvent;
    public static void InvokeStash(this Stash stash)
    {
        OnReadyStashEvent?.Invoke(new EventArgs(stash));
    }
    [SerializeField] public static Stash Backpack;
    [SerializeField] public static Stash Stash;

    public static void ClearEvents()
    { 
        OnChangeInventoryEvent = null;
        OnReadyStashEvent = null;
    }
}