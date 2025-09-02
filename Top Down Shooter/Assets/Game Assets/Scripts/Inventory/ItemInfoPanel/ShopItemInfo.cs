using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemInfo : ItemInfo
{
    [SerializeField] private ShopItem _shopItem;
    [SerializeField] private TextMeshProUGUI _buyOneCostTest;
    [SerializeField] private TextMeshProUGUI _buyStackCostTest;

    [SerializeField] private Button _buyOneButton;
    [SerializeField] private Button _buyStackButton;

    public void SetItem(ShopItem item)
    {
        _shopItem = item;
        _item = item.GetItem();

        SetUI();
    }

    protected override void SetUI()
    { 
        base.SetUI();
        SetButtons();
    }

    public void SetStatsText(string text)
    {
        _stats.text = text;
    }

    private void SetButtons()
    {
        _buyOneCostTest.text = $"{_item.Cost}";

        if (!IsAbleToBuyOne())
        {
            _buyOneButton.interactable = false;
        }

        if (_item.CanStack)
        {
            _buyStackCostTest.text = $"{_item.Cost*_item.MaxStackValue}";

            if (IsAbleToBuyStack())
            {
                _buyStackButton.interactable = true;
            }
            else
            {
                _buyStackButton.interactable = false;
            }
        }
        else
            _buyStackButton.gameObject.SetActive(false);
    }

    private bool IsAbleToBuyOne()
    {
        if (MainPlayer.Instance.Stats.IsEnoughMoney(_item.Cost))
            return true;

        return false;
    }

    private bool IsAbleToBuyStack()
    {
        if (MainPlayer.Instance.Stats.IsEnoughMoney(_item.Cost * _item.MaxStackValue))
            return true;

        return false;
    }

    public void BuyItemOne()
    {
        Stash stash = GlobalInventory.Instance.GetFreeSlotStash();
        if (stash != null)
        {
            if (IsAbleToBuyOne())
            {
                MainPlayer.Instance.Stats.ConsumeMoney(_item.Cost);
                stash.SpawnItem(GlobalInventory.Instance.ItemPrefab, _item);
                ClosePanel();
            }
        }
    }

    public void BuyItemStack()
    {
        Stash stash = GlobalInventory.Instance.GetFreeSlotStash();
        if (stash != null)
        {
            if (IsAbleToBuyStack())
            {
                MainPlayer.Instance.Stats.ConsumeMoney(_item.Cost * _item.MaxStackValue);
                stash.SpawnItem(GlobalInventory.Instance.ItemPrefab, _item, _item.MaxStackValue);
                ClosePanel();
            }
        }
    }
}
