using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : ItemBehaviour, IPointerClickHandler
{
    public ItemScriptableObject item;
    public int Count = 1;
    public override void Start()
    {
        base.Start();

        SetItemColor(item);
        SetIcon(item.Icon);
        SetText(item.CanStack, Count);

        gameObject.transform.localScale = Vector3.one; // почему то при изменении разрешения юнити скейлит айтем, хз нахуй он это делает -> эта fix инвалидный
    }
    public void UpdateText()
    {
        SetText(item.CanStack, Count);
    }
    public void OnEnable()
    {
        Start();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemInfoController.Instance.ShowInventoryItemInfoPanel(this);
    }
}
