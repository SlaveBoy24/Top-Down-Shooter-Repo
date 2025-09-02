using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.SimpleLocalization.Scripts;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ItemScriptableObject _item;

    [SerializeField] private Image _icon;
    [SerializeField] private Image _rarityIcon;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _cost;

    public void SetItem(ItemScriptableObject item)
    {
        _item = item;

        SetUI();
    }

    public ItemScriptableObject GetItem()
    { 
        return _item;
    }

    public void SetUI()
    {
        _icon.sprite = _item.Icon;

        _title.text = LocalizationManager.Localize(_item.NameKeyString);
        _description.text = LocalizationManager.Localize(_item.SmallDescriptionKeyString);
        _cost.text = $"{_item.Cost}";
        SetItemColor();
    }

    public void SetItemColor()
    {
        _rarityIcon.color = _item.GetRarityColor(0.5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemInfoController.Instance.ShowShopItemInfoPanel(this);
    }
}
