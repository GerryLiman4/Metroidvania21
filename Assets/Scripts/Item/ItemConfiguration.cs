using UnityEngine;

[CreateAssetMenu(fileName = "Item Configuration", menuName = "Create Item", order = 1)]
public class ItemConfiguration : ScriptableObject
{
    [SerializeField] private ItemId itemId;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private int maxAmount = 99;
    public bool isConsumAble = false;

    public int GetMaxAmount()
    {
        return maxAmount;
    }
    public ItemId GetItemId()
    {
        return itemId;
    }

    public string GetDescription()
    {
        return description;
    }
    public string GetName()
    {
        return itemName;
    }
    public Sprite GetItemIcon()
    {
        return itemIcon;
    }

}
