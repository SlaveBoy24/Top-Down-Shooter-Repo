using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemList", menuName = "Game Scriptable Objects/Shop Item List")]
public class ShopItemList : ScriptableObject
{
    public List<ItemScriptableObject> ItemList;
}
