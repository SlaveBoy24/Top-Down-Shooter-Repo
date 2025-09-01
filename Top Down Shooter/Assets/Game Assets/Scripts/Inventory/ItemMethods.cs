using System.Linq;
using UnityEngine;

public static class ItemMethods
{
    public static void SetItem(this Item item, Stash stash, int id)
    {
        stash.Items[id] = item;
    }
    public static int FindFreeSlot(this Stash stash)
    {
        var slots = stash.Items;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                return i;    
            }
        }

        return -1;
    }
    public static bool isFree(this Stash stash)
    {
        var slots = stash.Items;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                return false;    
            }
        }

        return true;
    }
    public static void SpawnItem(this Stash stash, GameObject itemPrefab, ItemScriptableObject item, int amount = 1)
    {
        int slotID = stash.FindFreeSlot();

        if (slotID == -1)
        {
            Debug.LogError($"Not enough space in {stash.gameObject.name}");
            return;
        }

        var itemGameObject = GameObject.Instantiate(itemPrefab);
        var itemScript = itemGameObject.GetComponent<Item>();

        if (!item.CanStack && amount > 1)
            amount = 1;

        itemScript.Count = amount;
        Debug.LogWarning(amount);
        SetupItem(itemScript, item, stash, slotID, itemGameObject, true);
    }

    public static void SpawnItemByKey(this Stash stash, GameObject itemPrefab, string key)
    {
        int slotID = stash.FindFreeSlot();

        if (slotID == -1)
        {
            Debug.LogError($"Not enough space in {stash.gameObject.name}");
            return;
        }

        var itemGameObject = GameObject.Instantiate(itemPrefab);
        var itemScript = itemGameObject.GetComponent<Item>();

        ItemScriptableObject item = null;
        ItemPool.All.TryGetValue(key, out item);

        if (item == null)
        {
            Debug.LogError($"Not found item by name {key}");
            return;
        }

        SetupItem(itemScript, item, stash, slotID, itemGameObject, true);
    }

    public static void SpawnItemByKeyWithID(this Stash stash, GameObject itemPrefab, string key, int slotID, int count)
    {
        var itemGameObject = GameObject.Instantiate(itemPrefab);
        var itemScript = itemGameObject.GetComponent<Item>();

        ItemScriptableObject item = null;
        ItemPool.All.TryGetValue(key, out item);

        if (item == null)
        {
            Debug.LogError($"Not found item by name {key}");
            return;
        }

        itemScript.Count = count;

        SetupItem(itemScript, item, stash, slotID, itemGameObject, false);
    }

    public static void SetupItem(Item itemScript, ItemScriptableObject item, Stash stash,
                                 int slotID, GameObject itemGameObject, bool isUpdate)
    {
        itemScript.item = item;

        itemScript.enabled = true;

        var slots = stash.GridParent.GetComponentsInChildren<InventorySlot>(true);

        foreach (InventorySlot s in slots)
        {
            if (s.SlotID == slotID)
            {
                itemGameObject.transform.SetParent(s.gameObject.transform);
            }
        }

        itemScript.SetItem(stash, slotID);

        if(isUpdate)
            GlobalStashes.Invoke(stash);
    }

    public static void SetNullItem(this Stash stash, int id)
    {
        stash.Items[id] = null;
    }
}