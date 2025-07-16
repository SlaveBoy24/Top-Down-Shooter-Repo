using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour
{
    [HideInInspector] public static DropdownController Instance;
    public GameObject DropdownObject;
    public List<Button> Buttons;
    public Item ActiveItem;
    public int Offset;
    public RectTransform CanvasRectTransform;


    private void Start()
    {
        Instance = this;

        Buttons = DropdownObject.GetComponentsInChildren<Button>().ToList();

        Buttons.RemoveAt(0);

        DropdownObject.SetActive(false);
    }

    public void Init(Vector3 position, Item itemObject)
    {
        ClearPrevious();

        ActiveItem = itemObject;

        Vector3 offset = new Vector3(
            + Offset,
            - Offset
        );

        DropdownObject.SetActive(true);

        if (!itemObject.item.CanStack)
        {
            Buttons[1].gameObject.SetActive(false);
        }
        else
        {
            if (itemObject.Count == 1)
                Buttons[1].gameObject.SetActive(false);
        }

        DropdownObject.transform.position = offset + position;

        ReturnToCanvas();
    }

    private void ReturnToCanvas()
    {
        RectTransform rectTransform = DropdownObject.transform as RectTransform;

        Rect canvasRect = CanvasRectTransform.rect;
        canvasRect.xMin += Offset;
        canvasRect.xMax -= Offset;
        canvasRect.yMin += Offset;
        canvasRect.yMax -= Offset;

        Vector2 currentPos = rectTransform.anchoredPosition;
        Vector2 halfSize = rectTransform.sizeDelta;

        float minX = canvasRect.xMin + halfSize.x;
        float maxX = canvasRect.xMax - halfSize.x;
        float minY = canvasRect.yMin + halfSize.y;
        float maxY = canvasRect.yMax - halfSize.y;

        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(currentPos.x, minX, maxX),
            Mathf.Clamp(currentPos.y, minY, maxY));

        rectTransform.anchoredPosition = clampedPosition;
    }

    private void ClearPrevious()
    {
        foreach (Button button in Buttons)
        {
            button.gameObject.SetActive(true);
        }

        ActiveItem = null;
    }
}
