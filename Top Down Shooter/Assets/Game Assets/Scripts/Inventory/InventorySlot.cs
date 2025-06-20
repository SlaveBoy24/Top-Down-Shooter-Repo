using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public ItemType Type = ItemType.None;
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount != 0) return;
        
        GameObject dropped = eventData.pointerDrag;
        Item itemBehaviour = dropped.GetComponent<Item>();
        if (itemBehaviour == null) return;

        if ((itemBehaviour.item.Type != Type) && (Type != ItemType.None)) return;

        itemBehaviour.ParentAfterDrag = transform;
    }
}
