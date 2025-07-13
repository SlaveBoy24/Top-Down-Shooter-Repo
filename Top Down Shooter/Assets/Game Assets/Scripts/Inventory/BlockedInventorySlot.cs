
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockedInventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Ты еблан у тебя слоты не куплены");
    }
}
