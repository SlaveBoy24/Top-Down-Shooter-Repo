using UnityEngine;
using System.Collections.Generic;

public enum LootLevel
{ 
    Common = 0,
    Rare = 25,
    Epic = 50,
    Myth = 75,
}

[System.Serializable]
public struct LootItem
{
    public ItemScriptableObject Item;
    public int MinAmount;
    public int MaxAmount;
    public float Chance;
}

[System.Serializable]
public struct LootListByLevel
{
    public LootLevel Level;
    public List<LootItem> List;
}

public class LootListStriptable : ScriptableObject
{
    public List<LootListByLevel> LootList;

    public List<LootItem> GetListByLevel(int level = 0)
    {
        if (LootList.Count == 1)
            return LootList[0].List;

        List<LootItem> list = null;

        foreach (LootListByLevel item in LootList)
        {
            if (level >= (int)item.Level)
                list = item.List;
        }

        return list;
    }
}
