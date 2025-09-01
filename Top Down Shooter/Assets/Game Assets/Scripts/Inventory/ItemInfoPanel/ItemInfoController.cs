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
        foreach (TypedInfoPanel panel in _typedPanels)
        {
            if (panel.Types.Contains(itemSlot.item.Type))
            {
                ItemInfo itemInfoPanel = Instantiate(panel.Panel, _infoPanelContainer);
                itemInfoPanel.SetItem(itemSlot);
                break;
            }
        }
    }

    public void ShowShopItemInfoPanel(ShopItem shopItem)
    {
        if (shopItem.GetItem() != null)
        {
            ShopItemInfo itemInfoPanel = Instantiate(ShopInfoPanel, _infoPanelContainer);
            itemInfoPanel.SetItem(shopItem);
        }
    }
}
