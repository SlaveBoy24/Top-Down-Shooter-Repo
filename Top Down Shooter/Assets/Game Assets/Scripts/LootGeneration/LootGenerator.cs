using UnityEngine;
using System.Collections.Generic;

public class LootGenerator : MonoBehaviour
{
    [SerializeField] private LootListStriptable _lootList;

    public List<StashItem> GenerateLoot(int level = 0)
    {
        List<LootItem> list = _lootList.GetListByLevel(level);
        list = RandomizeLoot(list, Random.Range(1, 4));

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

        for (int i = 0; i < amount; i++)
        {
            if (list.Count == 0)
                break;

            float maxIndex = GetMaxRandIndex(list);
            float randIndex = Random.Range(0f, maxIndex);

            float indexCount = 0;
            foreach (LootItem item in list)
            {
                indexCount += item.Chance;
                if (indexCount > randIndex)
                { 
                    newList.Add(item);
                    list.Remove(item);
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
