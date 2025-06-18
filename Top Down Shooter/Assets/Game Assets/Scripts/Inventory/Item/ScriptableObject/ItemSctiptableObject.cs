using UnityEngine;

[CreateAssetMenu(fileName = "ItemSctiptableObject", menuName = "Game Scriptable Objects/ItemSctiptableObject")]
public class ItemSctiptableObject : ScriptableObject
{
    public ItemType Type;
    public ItemValueType ValueType;
    public Sprite Icon;
    public int Cost;
    // Medical
    public float Heal;
    // Armour
    public float SlowDownMovementPercent;
    public float DamageBlockPercent;
    // Backpack
    public int SlotCount;
    // Weapon
    public int DamageDeal;
    public float ArmourPenetration;
    public int MaxBulletCount;
}


