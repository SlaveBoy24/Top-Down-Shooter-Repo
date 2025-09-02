using UnityEngine;

public static class ItemRarity
{
    public static Color GetRarityColor(this ItemScriptableObject _item, float alphaValue)
    {
        string color = "";
        switch (_item.ValueType)
        {
            case ItemValueType.Common:
                color = "#ADA6A6";
                break;
            case ItemValueType.Uncommon:
                color = "#ADDBAB";
                break;
            case ItemValueType.Rare:
                color = "#6F9FD4";
                break;
            case ItemValueType.Epic:
                color = "#8564D9";
                break;
            case ItemValueType.Mystical:
                color = "#C15755";
                break;
            case ItemValueType.Legendary:
                color = "#E0B636";
                break;
        }
        Color newCol;
        if (UnityEngine.ColorUtility.TryParseHtmlString(color, out newCol))
        {
            newCol.a = alphaValue;
            return newCol;
        }

        return new Color(1, 1, 1, 0.2f);
    }
}
