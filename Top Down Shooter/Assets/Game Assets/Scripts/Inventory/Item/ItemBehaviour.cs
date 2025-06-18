using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]public Image ImageItem;
    [HideInInspector]public Transform ParentAfterDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        ImageItem.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentAfterDrag);
        ImageItem.raycastTarget = true;
    }
    public void SetItemColor(ItemValueType itemValueType)
    {
        string color = "#ADA6A6";
        switch (itemValueType)
        {
            case ItemValueType.Rare:
                color = "#7C40D1";
                break;
            case ItemValueType.ExtraRare:
                color = "#E7DA1D";
                break;
            case ItemValueType.Insane:
                color = "#EC0808";
                break;
        }
        Color newCol;
        newCol.a = 0.2f;
        if (ColorUtility.TryParseHtmlString(color, out newCol))
        {
            ImageItem.color = newCol;
        }
        
    }
    public void SetIcon(Sprite sprite)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = sprite;    
    }
    public virtual void Start()
    {
        ImageItem = GetComponent<Image>();
    }
    public virtual void Init() { }
    public virtual void Use() { }
}

public enum ItemValueType
{
    Basic = 0,
    Rare = 1,
    ExtraRare = 2,
    Insane = 3
}

public enum ItemType
{
    None = 0,
    Medical = 1,
    ArmourHead = 21,
    ArmourChest = 22,
    ArmourLegs = 23,
    Backpack = 3,
    WeaponMain = 41,
    WeaponSecondary = 42
}