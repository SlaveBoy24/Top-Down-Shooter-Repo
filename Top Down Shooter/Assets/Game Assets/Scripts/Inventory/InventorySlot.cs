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
        if (transform.childCount != 0) return;

        GameObject dropped = eventData.pointerDrag;
        Item itemBehaviour = dropped.GetComponent<Item>();
        InventorySlot PastParent = null;

        if (itemBehaviour == null) return;

        if (itemBehaviour.ParentAfterDrag != null)
            PastParent = itemBehaviour.ParentAfterDrag.GetComponent<InventorySlot>();

        if ((itemBehaviour.item.Type != Type) && (Type != ItemType.None)) return;

        itemBehaviour.ParentAfterDrag = transform;

        if (PastParent != null)
            PastParent.Stash.SetNullItem(PastParent.SlotID);

        itemBehaviour.SetItem(Stash, SlotID);

        GlobalStashes.Invoke(Stash, PastParent.Stash);
    }
}
