using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.SimpleLocalization.Scripts;

public class ShopManager : MonoBehaviour
{
    [Header("Switch Panel")]
    [SerializeField] private GameObject _sellPanel;
    [SerializeField] private Transform _sellButton;
    [SerializeField] private GameObject _buyPanel;
    [SerializeField] private Transform _buyButton;

    [Header("Selling Panel")]
    [SerializeField] private Stash _globalInvetoryStash;
    [SerializeField] private Stash _stash;
    [SerializeField] private Stash _sellingStash;

    [SerializeField] private TextMeshProUGUI _sellingButtonText;
    [SerializeField] private string _sellButtonTextKey;
    [SerializeField] private string _sellButtonNullTextKey;
    [SerializeField] private int _totalSellingSum;

    [Header("Buy Panel")]
    [SerializeField] private ShopItemList _shopItemList;
    [SerializeField] private ShopItem _shopItemPrefab;
    [SerializeField] private List<ShopItem> _shopItems;
    [SerializeField] private Transform _shopItemContainer;


    public void Initialize()
    {
        SwitchPanel("sell");
        _sellingStash.Initialize();
        InitializeInventoryStash();
        UpdateSellingTotalSum();
    }

    public void SwitchPanel(string panelName)
    {
        if (panelName == "sell")
        {
            _buyPanel.SetActive(false);
            _sellPanel.SetActive(true);

            _buyButton.localScale = Vector3.one;
            _sellButton.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        }
        else
        {
            _sellPanel.SetActive(false);
            _buyPanel.SetActive(true);

            _buyButton.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            _sellButton.localScale = Vector3.one;
        }
    }

    public void OnEnable()
    {
        InitializeInventoryStash();
        GlobalStashes.OnChangeInventoryEvent += OnChangeStashEvent;
    }

    public void OnDisable()
    {
        GlobalInventory.Instance.UpdateStash();
        GlobalStashes.OnChangeInventoryEvent -= OnChangeStashEvent;
    }
    #region Shop Panel
    private void InitializeInventoryStash()
    {
        _globalInvetoryStash.Initialize();
        _stash.Initialize();
    }

    public void SellItems()
    {
        if (_totalSellingSum > 0)
        {
            Debug.LogWarning($"Total selling sum - {_totalSellingSum}");
            MainPlayer.Instance.Stats.AddMoney(_totalSellingSum);

            if (PlayerPrefs.HasKey(_sellingStash.gameObject.name))
                PlayerPrefs.DeleteKey(_sellingStash.gameObject.name);

            _sellingStash.Initialize();
            InitializeInventoryStash();
            UpdateSellingTotalSum();
        }
        else
        {
            Debug.LogWarning("No items in selling panel!");
        }
    }

    private void OnChangeStashEvent(GlobalStashes.EventArgs e)
    {
        if (e.Stash.transform.gameObject.name == "Stash")
        {
            InitializeInventoryStash();
        }
        
        if (e.Stash.transform.gameObject.name == "Selling Stash")
        {
            UpdateSellingTotalSum();
        }
        else if (e.PastStash)
        {
            if (e.PastStash.transform.gameObject.name == "Selling Stash")
                UpdateSellingTotalSum();
        }
    }

    private void UpdateSellingTotalSum()
    {
        int totalSum = 0;
        Item[] items = _sellingStash.Items;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                int amount = 1;
                if (items[i].item.CanStack && items[i].Count > 0)
                    amount = items[i].Count;

                totalSum += items[i].item.Cost * amount;
            }
        }

        _totalSellingSum = totalSum;

        SetSellingUI();
    }

    private void SetSellingUI()
    {
        if (_totalSellingSum > 0)
        {
            string text = LocalizationManager.Localize(_sellButtonTextKey);

            text = string.Format(text, (int)_totalSellingSum);

            _sellingButtonText.text = text;
        }
        else
        {
            _sellingButtonText.text = LocalizationManager.Localize(_sellButtonNullTextKey);
        }
    }
    #endregion

    public void BuildShopItemList(List<ItemType> types)
    {
        ClearShopItemList();

        foreach (ItemScriptableObject item in _shopItemList.ItemList)
        {
            if (types.Contains(item.Type))
            {
                ShopItem shopItem = Instantiate(_shopItemPrefab, _shopItemContainer);
                shopItem.SetItem(item);
                _shopItems.Add(shopItem);
            }
        }
    }

    private void ClearShopItemList()
    {
        foreach (ShopItem item in _shopItems)
        {
            Destroy(item.gameObject);
        }

        _shopItems = new List<ShopItem>();
    }
}
