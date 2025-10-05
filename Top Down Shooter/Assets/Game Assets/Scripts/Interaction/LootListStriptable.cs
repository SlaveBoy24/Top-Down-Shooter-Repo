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

[CreateAssetMenu(fileName = "LootListScriptable", menuName = "Loot Lists/new LootListScriptable")]
public class LootListStriptable : ScriptableObject
{
    public List<LootListByLevel> LootList;

    public List<LootItem> GetListByLevel(int level = 0)
    {
        Debug.Log("getting list");
        if (LootList.Count == 1)
        {
            Debug.Log("first list");
            return LootList[0].List;
        }

        List<LootItem> list = null;

        Debug.Log("loop list");
        foreach (LootListByLevel item in LootList)
        {
            Debug.Log($"{level} - {(int)item.Level}");
            if (level >= (int)item.Level)
            {
                list = item.List;
            }
        }

        return list;
    }
}
