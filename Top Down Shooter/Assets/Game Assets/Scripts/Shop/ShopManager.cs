using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Stash _stash;
    [SerializeField] private Stash _sellingStash;

    public void Initialize()
    {
        _sellingStash.Initialize();
        InitializeInventoryStash();
    }

    public void OnEnable()
    {
        InitializeInventoryStash();
    }

    private void InitializeInventoryStash()
    { 
        _stash.Initialize();
    }

    public void OnDisable()
    {
        GlobalInventory.Instance.UpdateStash();
    }
}
