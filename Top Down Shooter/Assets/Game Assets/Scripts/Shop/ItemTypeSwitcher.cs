using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemTypeSwitcher : MonoBehaviour
{
    [SerializeField] private ShopManager _shopManager;
    [SerializeField] private List<ItemType> _selectedTypes;
    [SerializeField] private List<ItemTypeSwitchButton> _buttons;
    [SerializeField] private Color _enabledColor;
    [SerializeField] private Color _disabledColor;

    private void OnEnable()
    {
        SwitchType(_buttons[0]);
    }

    public void SwitchType(ItemTypeSwitchButton selectedButton)
    {
        if (selectedButton.GetItemTypes() == _selectedTypes)
            return;

        foreach (ItemTypeSwitchButton button in _buttons)
        {
            if (button == selectedButton)
            {
                button.UpdateColor(_enabledColor);
                _selectedTypes = button.GetItemTypes();
                continue;
            }

            button.UpdateColor(_disabledColor);
        }

        OnItemTypeChanged();
    }

    private void OnItemTypeChanged()
    {
        _shopManager.BuildShopItemList(_selectedTypes);
    }
}