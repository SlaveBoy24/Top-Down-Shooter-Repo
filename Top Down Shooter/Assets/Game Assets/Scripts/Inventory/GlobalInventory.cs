using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalInventory : MonoBehaviour
{
    [HideInInspector]public static GlobalInventory Instance;
    public GameObject ItemPrefab;
    private void Awake()
    {
        Instance = this;
        var stashes = GetComponentsInChildren<Stash>();
        GlobalStashes.Backpack = stashes[6];
        GlobalStashes.Stash = stashes[7];

        GlobalStashes.OnChangeInventoryEvent += OnChangeInventoryEvent;
        GlobalStashes.OnReadyStashEvent += OnReadyStashEvent;
    }

    private void OnReadyStashEvent(GlobalStashes.EventArgs e)
    {
        Debug.Log($"Stash: {e.Stash.transform.gameObject.name} invoke");

        if (!PlayerPrefs.HasKey(e.Stash.transform.gameObject.name))
            return;

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
                    OnChangeEquipmentEvent(stash);
                    break;
            }
        }
    }

    private void OnChangeEquipmentEvent(Stash e)
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
                if (e.Items[0] == null)
                {
                    Debug.Log($"OnChangeEquipmentEvent({e.transform.gameObject.name}) - ITEM NULL");
                    return;
                }

                Debug.Log($"OnChangeEquipmentEvent({e.transform.gameObject.name}) - {e.Items[0].item.name.CamelToSnake()}");
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
}