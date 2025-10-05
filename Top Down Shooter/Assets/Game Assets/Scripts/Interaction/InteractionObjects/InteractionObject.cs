using UnityEngine;
using System.Collections.Generic;

public enum InteractionType
{ 
    Item,
    Stash,
    Door,
    Terminal
}

public enum InteractionLockedStatus
{ 
    Unlocked,
    Locked,
    LockedByPerk,
    LockedEnemy
}

public class InteractionObject : MonoBehaviour
{
    [SerializeField] private InteractionType _type;
    [SerializeField] protected InteractionLockedStatus _lockedStatus;
    [SerializeField] private LootListStriptable _lootList;

    public InteractionType GetInteractionType()
    { 
        return _type;
    }

    public InteractionLockedStatus GetInteractionLockedStatus()
    {
        return _lockedStatus;
    }

    public virtual void Unlock()
    { 
    
    }

    public virtual void Interact()
    { 
    
    }

    public virtual bool IsAbleToInteract()
    {
        return false;
    }

    public List<StashItem> GenerateLoot(int level = 0, int maxItemAmount = 0)
    {
        Debug.Log("GENERATE LOOT");
        List<LootItem> list = _lootList.GetListByLevel(level);
        Debug.Log($"list count - {list.Count}");
        if (maxItemAmount == 0)
            list = RandomizeLoot(list, Random.Range(1, 5));
        else
            list = RandomizeLoot(list, Random.Range(1, maxItemAmount+1));

        List<StashItem> loot = new List<StashItem>();

        for (int i = 0; i < list.Count; i++)
        {
            StashItem item = new StashItem();
            item.Item = list[i].Item;

            if (item.Item.CanStack)
            {
                int maxAmount = list[i].MaxAmount == 0 ? item.Item.MaxStackValue : list[i].MaxAmount;

                item.Amount = Random.Range(list[i].MinAmount, maxAmount);
            }
            else
            {
                item.Amount = 1;
            }

            loot.Add(item);
        }

        return loot;
    }

    private List<LootItem> RandomizeLoot(List<LootItem> list, int amount = 1)
    {
        List<LootItem> newList = new List<LootItem>();
        List<LootItem> copyList = new List<LootItem>(list);

        for (int i = 0; i < amount; i++)
        {
            Debug.Log(copyList.Count);
            if (copyList.Count == 0)
                break;

            float maxIndex = GetMaxRandIndex(copyList);
            float randIndex = Random.Range(0f, maxIndex);

            float indexCount = 0;
            foreach (LootItem item in copyList)
            {
                Debug.Log("Add item");
                indexCount += item.Chance;
                if (indexCount > randIndex)
                {
                    newList.Add(item);
                    copyList.Remove(item);
                    break;
                }
            }
        }

        return newList;
    }

    private float GetMaxRandIndex(List<LootItem> list)
    {
        float index = 0;
        foreach (LootItem item in list)
            index += item.Chance;

        return index;
    }
}
