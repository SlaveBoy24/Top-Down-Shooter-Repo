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
        string color = "#ADA6A6";
        switch (_item.ValueType)
        {
            case ItemValueType.Rare:
                color = "#7C40D1";
                break;
            case ItemValueType.Epic:
                color = "#E7DA1D";
                break;
            case ItemValueType.Mystical:
                color = "#EC0808";
                break;
        }
        Color newCol;
        if (ColorUtility.TryParseHtmlString(color, out newCol))
        {
            newCol.a = 0.2f;
            _rarityIcon.color = newCol;
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemInfoController.Instance.ShowShopItemInfoPanel(this);
    }
}
