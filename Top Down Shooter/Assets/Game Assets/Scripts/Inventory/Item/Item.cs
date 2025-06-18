using UnityEngine;

public class Item : ItemBehaviour
{
    public ItemSctiptableObject item;
    public override void Start()
    {
        base.Start();

        SetItemColor(item.ValueType);
        SetIcon(item.Icon);
    }

    public void OnEnable()
    {
        Start();
    }

}
