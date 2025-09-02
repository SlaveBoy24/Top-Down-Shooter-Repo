using UnityEngine;
using Assets.SimpleLocalization.Scripts;

public class WeaponInfo : ItemInfo
{
    public override string GetStatsString()
    {
        string stats_text = GetStatsLocalizedText();
        string localized_item_rarity = LocalizationManager.Localize($"{(int)_item.ValueType}_item_type");

        stats_text = string.Format(
            stats_text,
            localized_item_rarity,
            $"{_item.DamageDeal}",
            $"{_item.ArmourPenetration}",
            $"{_item.MaxBulletCount}",
            $"{_item.Cost}"
        );

        return stats_text;
    }
}
