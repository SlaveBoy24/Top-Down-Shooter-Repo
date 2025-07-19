using UnityEngine;
using System.Collections.Generic;

public class Interactor : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<InteractionObject> _items;
    [SerializeField] private List<InteractionObject> _doors;
    [SerializeField] private List<InteractionObject> _stashes;

    [Header("Buttons and Panels")]
    [SerializeField] private GameObject _unlockButton;
    [SerializeField] private GameObject _interactButton;
    [SerializeField] private GameObject _lootPanel;

    [Header("Controllers")]
    [SerializeField] private LootTableController _lootTableController;
    [SerializeField] private LockBreaking _lockBreakingMiniGame;

    public void Interact()
    {
        if (_items.Count > 0)
        {
            _items[0].Interact();
            _items.RemoveAt(0);
        }
        else
        {
            _doors[0].Interact();
            _doors.RemoveAt(0);
        }
        
        UpdateUI();
    }

    public void Unlock()
    {
        if (GetLockedDoorOrStash() == -1)
        {
            UpdateUI();
            return;
        }

        int stashIndex = GetElementWithLockedStatus(_stashes, InteractionLockedStatus.Locked);
        if (stashIndex != -1)
        {
            _lockBreakingMiniGame.Initialize();
        }
        else
        {
            int doorIndex = GetElementWithLockedStatus(_doors, InteractionLockedStatus.Locked);
            if (doorIndex == -1)
            {
                UpdateUI();
                return;
            }
        }
    }

    #region InteractorUiLogic
    private void UpdateUI()
    {
        DisableButtons();

        if (_items.Count > 0 || GetUnlockedDoorIndex() != -1)
        {
            _interactButton.SetActive(true);
        }

        if (GetLockedDoorOrStash() != -1)
        { 
            _unlockButton.SetActive(true);
        }

        _lootTableController.SetItemList(_stashes);
    }

    private int GetUnlockedDoorIndex()
    {
        return GetElementWithLockedStatus(_doors, InteractionLockedStatus.Unlocked);
    }

    private int GetLockedDoorOrStash()
    {
        List<InteractionObject> allObjects = new List<InteractionObject>();
        allObjects.AddRange(_stashes);
        allObjects.AddRange(_doors);

        return GetElementWithLockedStatus(allObjects, InteractionLockedStatus.Locked);
    }

    private void DisableButtons()
    {
        _unlockButton.SetActive(false);
        _interactButton.SetActive(false);
        _lootPanel.SetActive(false);
    }
    #endregion

    public void AddInterationObject(GameObject obj)
    {
        InteractionObject intercationObject = obj.GetComponent<InteractionObject>();
        List<InteractionObject> list = GetList(intercationObject);
        if (list == null)
            return;

        if (list.Contains(intercationObject))
            return;

        if (intercationObject.IsAbleToInteract())
            list.Add(intercationObject);

        UpdateUI();
    }

    public void RemoveInteractionObject(GameObject obj)
    {
        InteractionObject intercationObject = obj.GetComponent<InteractionObject>();
        List<InteractionObject> list = GetList(intercationObject);

        if (list == null)
            return;

        if (!list.Contains(intercationObject))
            return;

        list.Remove(intercationObject);

        UpdateUI();
    }

    private int GetElementWithLockedStatus(List<InteractionObject> list, InteractionLockedStatus status)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetInteractionLockedStatus() == status)
                return i;
        }

        return -1;
    }

    private List<InteractionObject> GetList(InteractionObject obj)
    {
        switch (obj.GetInteractionType())
        {
            case InteractionType.Item:
                return _items;
            case InteractionType.Stash:
                return _stashes;
            case InteractionType.Door:
                return _doors;
            default:
                return null;
        }
    }
}
