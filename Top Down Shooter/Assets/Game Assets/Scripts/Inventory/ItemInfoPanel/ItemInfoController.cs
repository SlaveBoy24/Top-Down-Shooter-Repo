using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TypedInfoPanel
{
    public List<ItemType> Types;
    public ItemInfo Panel;
}

public class ItemInfoController : MonoBehaviour
{
    public static ItemInfoController Instance;
    [SerializeField] private Transform _infoPanelContainer;
    [SerializeField] private List<TypedInfoPanel> _typedPanels;
    [SerializeField] private ShopItemInfo ShopInfoPanel;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ShowInventoryItemInfoPanel(Item itemSlot)
    {
        ItemInfo panelPrefab = GetInfoPanel(itemSlot.item);

        if (panelPrefab == null)
            return;

        ItemInfo itemInfoPanel = Instantiate(panelPrefab, _infoPanelContainer);
        itemInfoPanel.SetItem(itemSlot);
    }

    private ItemInfo GetInfoPanel(ItemScriptableObject item)
    {
        foreach (TypedInfoPanel panel in _typedPanels)
        {
            if (panel.Types.Contains(item.Type))
            {
                return panel.Panel;
            }
        }

        return null;
    }

    public void ShowShopItemInfoPanel(ShopItem shopItem)
    {
        if (shopItem.GetItem() != null)
        {
            ItemInfo panelPrefab = GetInfoPanel(shopItem.GetItem());
            panelPrefab.SetItem(shopItem.GetItem());
            string stats_text = panelPrefab.GetStatsString();

            if (panelPrefab == null)
                return;

            ShopItemInfo itemInfoPanel = Instantiate(ShopInfoPanel, _infoPanelContainer);
            itemInfoPanel.SetItem(shopItem);
            itemInfoPanel.SetStatsText(stats_text);
        }
    }
}
