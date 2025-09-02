using ExitGames.Client.Photon.StructWrapping;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Image ImageItem;
    [HideInInspector] public TMP_Text TextItem;
    [HideInInspector] public Transform ParentAfterDrag;

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
    public void SetItemColor(ItemScriptableObject _item)
    {
        ImageItem.color = _item.GetRarityColor(0.5f);
    }
    public void SetIcon(Sprite sprite)
    {
        transform.GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    public void SetText(bool CanStack, int Count)
    {
        if (!CanStack)
        {
            TextItem.enabled = false;
            return;
        }

        if (Count > 1)
        {
            TextItem.text = $"{Count}";
        }
        else
        {
            TextItem.text = "";
        }
    }

    public virtual void Start()
    {
        ImageItem = GetComponent<Image>();
        TextItem = GetComponentInChildren<TMP_Text>();
    }
    //public virtual void Init() { }
    public virtual void Use() { }
}

public enum ItemValueType
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
    Epic = 3,
    Mystical = 4,
    Legendary = 5,
}

public enum ItemType
{
    None = 0,
    Medical = 1,
    EquipmentHead = 21,
    EquipmentChest = 22,
    EquipmentLegs = 23,
    Backpack = 3,
    WeaponMain = 41,
    WeaponSecondary = 42,
    Other = 5,
}