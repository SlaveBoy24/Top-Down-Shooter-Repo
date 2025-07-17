using UnityEngine;
using System.Collections.Generic;

public class LootTableController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _lootTablePanel;
    [SerializeField] private Transform _itemsContainer;

    [Header("Logic")]
    [SerializeField] private bool _isOpened;
    [SerializeField] private List<InteractionStash> _stashes;
    [SerializeField] private List<LootTableUiElement> _uiItems;
    [SerializeField] private GameObject _uiItemPrefab;

    private void Start()
    {
        _isOpened = true;
        _lootTablePanel.SetActive(true);
    }

    public void SetLootTableActiveState()
    {
        _isOpened = !_isOpened;
        _lootTablePanel.SetActive(_isOpened);
    }

    public void SetItemList(List<InteractionObject> stashes)
    {
        if (stashes.Count == 0)
        { 
            gameObject.SetActive(false);
            return;
        }

        _stashes.Clear();

        foreach (InteractionObject stash in stashes)
        {
            if (stash.GetInteractionLockedStatus() == InteractionLockedStatus.Unlocked)
                _stashes.Add(stash.GetComponent<InteractionStash>());
        }

        if (_stashes.Count > 0)
        {
            ResetList();
            gameObject.SetActive(true);
        }

        foreach (InteractionStash stash in _stashes)
        {
            List<StashItem> items = stash.GetList();

            foreach (StashItem item in items)
            {
                SetUiItem(item, stash);
            }
        }
    }

    private void SetUiItem(StashItem item, InteractionStash stash)
    {
        LootTableUiElement uiItem = Instantiate(_uiItemPrefab, _itemsContainer).GetComponent<LootTableUiElement>();
        _uiItems.Add(uiItem);
        uiItem.SetElement(item, stash);
    }

    private void ResetList()
    {
        foreach (LootTableUiElement item in _uiItems)
        {
            Destroy(item.gameObject);
        }

        _uiItems.Clear();
    }
}
