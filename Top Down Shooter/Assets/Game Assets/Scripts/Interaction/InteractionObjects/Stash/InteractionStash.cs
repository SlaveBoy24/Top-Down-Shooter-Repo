using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class StashItem
{
    public ItemScriptableObject Item;
    public int Amount;
}

public class InteractionStash : InteractionObject
{
    [SerializeField] private List<StashItem> _items;

    public override bool IsAbleToInteract()
    {
        UpdateList();

        if (_items.Count > 0)
            return true;

        return false;
    }

    private void UpdateList()
    {
        foreach (StashItem item in _items)
        {
            if (item == null)
                _items.Remove(item);
        }
    }

    public override void Unlock()
    {
        _lockedStatus = InteractionLockedStatus.Unlocked;
    }

    public List<StashItem> GetList()
    { 
        return _items;
    }

    public void RemoveItem(StashItem item)
    { 
        _items.Remove(item);
    }
}
