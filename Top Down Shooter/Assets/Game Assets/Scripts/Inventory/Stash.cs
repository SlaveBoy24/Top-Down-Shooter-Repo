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

    [SerializeField] private Item[] _items;

    public Item[] Items { get => _items; }

    private void Awake()
    {
        Despawn();
        SetSize();
        SpawnSlots();
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

        this.InvokeStash();
    }

    private void Start()
    {
        if(gameObject.name != "Backpack")
            this.InvokeStash();
    }
    private void Despawn()
    {
        while (GridParent.transform.childCount > 0)
        {
            DestroyImmediate(GridParent.transform.GetChild(0).gameObject);
        }
    }
    private void SpawnSlots()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            var slot = Instantiate(SlotPrefab, GridParent);
            slot.GetComponentInChildren<GridLayoutGroup>().cellSize = new Vector2(CellSize, CellSize);
            var slot_settings = slot.GetComponentInChildren<InventorySlot>();
            slot_settings.Stash = this;
            slot_settings.SlotID = i;
            slot_settings.Type = SlotType;
        }
        for (int i = 0; i < BlockedSlotCount; i++)
        {
            var block_slot = Instantiate(BlockSlotPrefab, GridParent);
        }

        _items = new Item[SlotCount];
    }
    private int GetLineCount(int slot) => Mathf.CeilToInt((float)(slot) / SlotInLine);
    private void SetSize()
    {
        var gph = CellSize * GetLineCount(SlotCount + BlockedSlotCount);

        var rectGridParent = GridParent.GetComponent<RectTransform>();
        rectGridParent.sizeDelta = new Vector2(rectGridParent.sizeDelta.x, gph);

        var h = TitleH + gph;
        var rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, h);

        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }
}
