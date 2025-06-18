using System;
using UnityEngine;

public class Stash : MonoBehaviour
{
    public int TitleH = 75;
    public int CellSize = 125;
    public Transform GridParent;
    public GameObject SlotPrefab;
    public GameObject BlockSlotPrefab;
    public int SlotInLine = 8;

    public int SlotCount;
    public int BlockedSlotCount;

    private void Start()
    {
        Despawn();
        SetSize();
        SpawnSlots();
    }
    private void Despawn()
    {
        while (GridParent.transform.childCount > 0) {
            DestroyImmediate(GridParent.transform.GetChild(0).gameObject);
        }
    }
    private void SpawnSlots()
    {
        for (int i = 0; i < SlotCount; i++)
            Instantiate(SlotPrefab, GridParent);
        for (int i = 0; i < BlockedSlotCount; i++)
            Instantiate(BlockSlotPrefab, GridParent);
    }
    private int GetLineCount(int slot) => Mathf.CeilToInt((float)(slot)/SlotInLine);
    private void SetSize()
    {
        var gph = CellSize * GetLineCount(SlotCount+BlockedSlotCount);

        var rectGridParent = GridParent.GetComponent<RectTransform>();
        rectGridParent.sizeDelta = new Vector2(rectGridParent.sizeDelta.x, gph);

        var h = TitleH + gph;
        var rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, h);
    }
}
