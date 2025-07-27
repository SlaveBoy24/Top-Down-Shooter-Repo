using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Stash Stash;
    public int SlotID;
    public ItemType Type = ItemType.None;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Item itemBehaviour = dropped.GetComponent<Item>();
        InventorySlot PastParent = null;

        if (itemBehaviour == null) return;

        if (itemBehaviour.ParentAfterDrag != null)
            PastParent = itemBehaviour.ParentAfterDrag.GetComponent<InventorySlot>();

        if ((itemBehaviour.item.Type == ItemType.Backpack) && (PastParent.Stash.gameObject.name == "Backpack Slot"))
        {
            if (!GlobalStashes.Backpack.isFree())
            {
                NotificationManager.Instance.SendNotification("notification_clear_backpack");

                Debug.LogError("Empty ur backpack!");
                return;
            }
        }

        if ((PastParent.Stash.gameObject.name == "Backpack Slot") && (Stash.gameObject.name == "Backpack"))
        {
            NotificationManager.Instance.SendNotification("notification_bp_in_bp");

            Debug.LogError("You can't put a backpack in a backpack!");
            return;
        }

        if (transform.childCount != 0)
        {
            var childrenItem = transform.GetChild(0).GetComponent<Item>();

            if (childrenItem.item.name == itemBehaviour.item.name)
            {
                if (childrenItem.item.CanStack)
                {
                    if (childrenItem.item.MaxStackValue >= (childrenItem.Count + itemBehaviour.Count))
                    {
                        childrenItem.Count += itemBehaviour.Count;
                        PastParent.Stash.SetNullItem(PastParent.SlotID);

                        Destroy(itemBehaviour.gameObject);

                        childrenItem.UpdateText();

                        GlobalStashes.Invoke(Stash, PastParent.Stash);
                    }
                    else
                    {
                        Debug.LogWarning($"u stack bigger then limit {childrenItem.Count}+{itemBehaviour.Count}={childrenItem.Count + itemBehaviour.Count}>{childrenItem.item.MaxStackValue}");
                        return;
                    }
                }
            }

            return;
        }

        if ((itemBehaviour.item.Type != Type) && (Type != ItemType.None)) return;

        itemBehaviour.ParentAfterDrag = transform;

        if (PastParent != null)
            PastParent.Stash.SetNullItem(PastParent.SlotID);

        itemBehaviour.SetItem(Stash, SlotID);

        GlobalStashes.Invoke(Stash, PastParent.Stash);
    }
}
