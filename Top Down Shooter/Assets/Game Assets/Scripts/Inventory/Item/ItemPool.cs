using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private ItemPoolScriptableObject itemPoolScriptableObject;
    public static Dictionary<string, ItemSctiptableObject> All;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        itemPoolScriptableObject = ItemPoolScriptableObject.instance;

        All = new();

        foreach (LikeDict item in itemPoolScriptableObject.ItemsPoolList)
        {
            All[item.key] = item.value;
        }

        Debug.Log("Item Pool is Init");
    }
}
