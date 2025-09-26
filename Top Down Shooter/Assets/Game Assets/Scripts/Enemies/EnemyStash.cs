using UnityEngine;
using System.Collections.Generic;

public class EnemyStash : InteractionObject
{
    [SerializeField] private List<StashItem> _items;

    public override bool IsAbleToInteract()
    {
        UpdateList();

        if (_items.Count > 0 && _lockedStatus == InteractionLockedStatus.Unlocked)
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
        gameObject.SetActive(true);
        _lockedStatus = InteractionLockedStatus.Unlocked;
        FindAnyObjectByType<Interactor>().UpdateUI();
    }

    public List<StashItem> GetList()
    {
        return _items;
    }
}
