using UnityEngine;
using TMPro;
using Assets.SimpleLocalization.Scripts;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Stash _stash;
    [SerializeField] private Stash _sellingStash;

    [SerializeField] private TextMeshProUGUI _sellingButtonText;
    [SerializeField] private string _sellButtonTextKey;
    [SerializeField] private string _sellButtonNullTextKey;
    [SerializeField] private int _totalSellingSum;

    public void Initialize()
    {
        _sellingStash.Initialize();
        InitializeInventoryStash();
        UpdateSellingTotalSum();
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

    private void InitializeInventoryStash()
    { 
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
            UpdateSellingTotalSum();
        }
        else
        {
            Debug.LogWarning("No items in selling panel!");
        }
    }

    private void OnChangeStashEvent(GlobalStashes.EventArgs e)
    {
        if (e.Stash.transform.gameObject.name == "Selling Stash" || e.PastStash.transform.gameObject.name == "Selling Stash")
            UpdateSellingTotalSum();
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
}
