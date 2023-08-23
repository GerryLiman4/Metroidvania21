using UnityEngine;

public class DropItem : BaseItem
{
    [SerializeField] private bool canExpired;
    [SerializeField] private bool isFloating;
    [SerializeField] private bool isRotating;

    [SerializeField] private int amount;
    
    public int GetAmount()
    {
        return amount;
    }
    public ItemId GetItemId()
    {
        return itemConfiguration.GetItemId();
    }
    public void PickUp()
    {
        Destroy(gameObject);
    }
}
