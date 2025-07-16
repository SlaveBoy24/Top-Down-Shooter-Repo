using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class JSONStash
{
    [Serializable]
    public class ItemWrapper
    {
        public int sid; // slot id
        public string son; // scriptable object name 
        public int c; // count
        // почему такие имена? -> потому что в json они будут короче, тем самым json будет меньше байт 
        public ItemWrapper(int id, Item item)
        {
            sid = id;
            son = $"{item.item.name.CamelToSnake()}";
            c = item.Count;
        }
    }
    [Serializable]
    public class ItemsWrapper
    {
        public string sn; // stash name
        public List<ItemWrapper> i = new List<ItemWrapper>(); // items

        public ItemsWrapper(Stash stash)
        {
            sn = stash.transform.gameObject.name;
        }
    }
    public static string ItemsToJSON(this Stash stash)
    {
        ItemsWrapper wrapper = new ItemsWrapper(stash);

        for (int i = 0; i < stash.Items.Length; i++)
        {
            if (stash.Items[i] != null)
            {
                wrapper.i.Add(new ItemWrapper(i, stash.Items[i]));
            }
        }

        return JsonUtility.ToJson(wrapper);
    }

    public static ItemsWrapper JSONToItems(this string json)
    {
        return JsonUtility.FromJson<ItemsWrapper>(json);
    }
}
