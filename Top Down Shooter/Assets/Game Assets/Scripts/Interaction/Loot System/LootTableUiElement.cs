using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootTableUiElement : MonoBehaviour
{
    [Header("Logic")]
    [SerializeField] private LootTableController _controller;
    [SerializeField] private InteractionStash _parentStash;
    [SerializeField] private StashItem _stashItem;

    [Header("UI")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _amount;

    public void SetElement(StashItem stashItem, InteractionStash stash, LootTableController controller)
    {
        _controller = controller;
        _stashItem = stashItem;
        _parentStash = stash;

        SetUI();
    }

    private void SetUI()
    {
        _icon.sprite = _stashItem.Item.Icon;
        //_name.text = _stashItem.Item.Name;
        _amount.text = $"{_stashItem.Amount}";
    }
}
