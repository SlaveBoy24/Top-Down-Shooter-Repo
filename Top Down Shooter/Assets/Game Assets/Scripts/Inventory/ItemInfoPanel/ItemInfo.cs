using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.SimpleLocalization.Scripts;
using Unity.VisualScripting;

public class ItemInfo : MonoBehaviour
{
    [Header("Item Object")]
    [SerializeField] protected Item _itemSlot;
    [SerializeField] protected ItemScriptableObject _item;

    [Header("UI Elements")]
    [SerializeField] protected Image _itemIcon;
    [SerializeField] protected TextMeshProUGUI _title;
    [SerializeField] protected TextMeshProUGUI _stats;
    [SerializeField] protected TextMeshProUGUI _description;

    [Header("Rarity UI Elements")]
    [SerializeField] protected Image _itemRatity;
    [SerializeField] protected Image _titleBgColor;
    [SerializeField] protected Image _gradientColor;

    public void ClosePanel()
    { 
        Destroy(gameObject);
    }

    // only for getting stats for shop panel
    public void SetItem(ItemScriptableObject item)
    {
        _item = item;
    }

    public void SetItem(Item item)
    { 
        _itemSlot = item;
        _item = _itemSlot.item;

        SetUI();
    }

    protected virtual void SetUI()
    {
        _title.text = LocalizationManager.Localize(_item.NameKeyString);
        _itemIcon.sprite = _item.Icon;
        SetStats();
        _description.text = LocalizationManager.Localize(_item.FullDescriptionKeyString);
        SetRarityColors();
    }

    protected void SetRarityColors()
    {
        _itemRatity.color = _item.GetRarityColor(0.2f);
        _titleBgColor.color = _item.GetRarityColor(0.3f);
        _gradientColor.color = _item.GetRarityColor(0.03f);
    }

    protected void SetStats()
    {
        string stats_text = GetStatsString();

        _stats.text = stats_text;
    }

    public virtual string GetStatsString()
    {
        return "";
    }

    protected string GetStatsLocalizedText()
    {
        string localize_key = $"{(int)_item.Type}"[0] + "_stats_text";
        return LocalizationManager.Localize(localize_key);
    }
}
