using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTypeSwitchButton : MonoBehaviour
{
    [SerializeField] private ItemTypeSwitcher _switcher;
    [SerializeField] private List<ItemType> _itemTypes;
    [SerializeField] private Image _buttonSprite;
    [SerializeField] private Image _buttonIcon;

    public void SwitchItemType()
    {
        _switcher.SwitchType(this);
    }

    public List<ItemType> GetItemTypes()
    {
        return _itemTypes;
    }

    public void UpdateColor(Color color)
    {
        _buttonSprite.color = color;
        _buttonIcon.color = color;
    }
}
