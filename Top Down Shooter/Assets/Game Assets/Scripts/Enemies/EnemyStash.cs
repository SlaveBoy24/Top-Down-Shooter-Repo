using UnityEngine;
using System.Collections.Generic;

public class EnemyStash : InteractionStash
{
    [SerializeField] private Enemy _enemy;

    public override bool IsAbleToInteract()
    {
        UpdateList();

        if (_items.Count > 0 && _lockedStatus == InteractionLockedStatus.Unlocked)
            return true;

        return false;
    }

    public override void GenerateItems()
    {
        if (!_lootGenerated)
        {
            _items = GenerateLoot(_enemy.GetLevel());
            _lootGenerated = true;
        }
    }

    public override void Unlock()
    {
        gameObject.SetActive(true);
        _lockedStatus = InteractionLockedStatus.Unlocked;
        FindAnyObjectByType<Interactor>().UpdateUI();
    }
}
