using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stash : MonoBehaviour
{
    public int TitleH = 75;
    public int CellSize = 125;
    public Transform GridParent;
    public GameObject SlotPrefab;
    public GameObject BlockSlotPrefab;
    public int SlotInLine = 8;
    public ItemType SlotType = ItemType.None;
    public int DefaultSlotCount = 4;
    public int SlotCount;
    public int BlockedSlotCount;

    [SerializeField] private bool _isEquipSlot = false;

    [SerializeField] private Item[] _items;

    public Item[] Items { get => _items; }

    public void EquipItemUpdate()
    {
        if (_isEquipSlot)
        {
            Image image = this.GetComponentInChildren<Image>();

            if (Items[0] != null)
            {
                image.enabled = false;
            }
            else
            {
                image.enabled = true;
            }
        }
    }

    private void Awake()
    {
        /*Despawn();
        SetSize();
        SpawnSlots();*/
    }

    public void Initialize()
    {
        Despawn();
        SetSize();
        SpawnSlots();

        this.InvokeStash();
    }

    public void UpdateSlotCount(params int[] Count)
    {
        if (Count.Length > 0)
            SlotCount = Count[0];
        else
            SlotCount = DefaultSlotCount;

        Despawn();
        SetSize();
        SpawnSlots();

        GlobalStashes.Backpack.InvokeStash();
    }

    private void Start()
    {
        //if(gameObject.name != "Backpack")
           // this.InvokeStash();
    }
    private bool Despawn()
    {
        Transform[] childrens = GridParent.GetComponentsInChildren<Transform>(true);

        foreach (Transform children in childrens)
        {
            if (children == GridParent)
                continue;

            Destroy(children.gameObject);
        }

        return true;
    }
    private bool SpawnSlots()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            var slot = Instantiate(SlotPrefab, GridParent);
            slot.GetComponentInChildren<GridLayoutGroup>(true).cellSize = new Vector2(CellSize, CellSize);
            var slot_settings = slot.GetComponentInChildren<InventorySlot>(true);
            slot_settings.Stash = this;
            slot_settings.SlotID = i;
            slot_settings.Type = SlotType;
        }
        for (int i = 0; i < BlockedSlotCount; i++)
        {
            var block_slot = Instantiate(BlockSlotPrefab, GridParent);
        }

        _items = new Item[SlotCount];

        return true;
    }
    private int GetLineCount(int slot) => Mathf.CeilToInt((float)(slot) / SlotInLine);
    private bool SetSize()
    {
        var gph = CellSize * GetLineCount(SlotCount + BlockedSlotCount);

        var rectGridParent = GridParent.GetComponent<RectTransform>();
        rectGridParent.sizeDelta = new Vector2(rectGridParent.sizeDelta.x, gph);

        var h = TitleH + gph;
        var rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, h);

        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);

        return true;
    }
}
