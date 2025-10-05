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
    [SerializeField] protected List<StashItem> _items;
    [SerializeField] protected bool _lootGenerated;

    private void Start()
    {
        GenerateItems();
    }

    public virtual void GenerateItems()
    {
        if (!_lootGenerated)
        {
            _items = GenerateLoot();
            _lootGenerated = true;
        }
    }

    public override bool IsAbleToInteract()
    {
        UpdateList();

        if (_items.Count > 0)
            return true;

        return false;
    }

    public void UpdateList()
    {
        foreach (StashItem item in _items.ToArray())
        {
            if (item == null)
                _items.Remove(item);
            else if (item.Item == null)
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
}
