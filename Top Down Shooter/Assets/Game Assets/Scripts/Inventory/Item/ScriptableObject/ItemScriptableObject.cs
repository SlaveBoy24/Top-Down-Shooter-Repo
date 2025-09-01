using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "Game Scriptable Objects/ItemSctiptableObject")]
[Serializable]
public class ItemScriptableObject : ScriptableObject
{
    public string NameKeyString;
    public string FullDescriptionKeyString;
    public string SmallDescriptionKeyString;

    public ItemType Type;
    public ItemValueType ValueType;
    public Sprite Icon;
    public int Cost;

    public bool CanStack;
    public int MaxStackValue;

    // Medical
    public float Heal;
    // Armour
    public float SlowDownMovementPercent;
    public float DamageBlockPercent;
    public ClothModel ClothScriptableObject;
    // Backpack
    public int SlotCount;
    // Weapon
    public int DamageDeal;
    public float ArmourPenetration;
    public int MaxBulletCount;
    public WeaponScriptableObject GunSettings;
}


