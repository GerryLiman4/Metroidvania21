using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 20;
    [SerializeField] private Dictionary<ItemId, Inventory> inventoryList = new Dictionary<ItemId, Inventory>();
    [SerializeField] private CurrencyManager currencyManager;

    [SerializeField] private ItemPickUpFader itemPickUpFader;
#if UNITY_EDITOR
    [SerializeField] private GameObject[] prefabExamples;
    [SerializeField] private PlayerInputManager playerInput;
#endif
    private void Awake()
    {
#if UNITY_EDITOR
        playerInput.Inventory += OnInventoryOpen;
#endif
    }

    public bool AddItem(BaseItem item, int amount)
    {
        // there is downside that if item was in max capacity it could go out limit
        bool isSuccess = false;
        ItemId itemId = item.itemConfiguration.GetItemId();
        int currentCapacity = GetCurrentCapacity();

        if (!inventoryList.ContainsKey(itemId))
        {
            if (currentCapacity >= maxCapacity) return isSuccess;
            Inventory inventory = new Inventory(item, 0);
            inventoryList.Add(itemId, inventory);
        }

        inventoryList[itemId].AddAmount(amount);
        isSuccess = true;

        currentCapacity = GetCurrentCapacity();
        if (currentCapacity > maxCapacity)
        {
            DropItem(itemId, inventoryList[itemId].GetItemAmount() % inventoryList[itemId].item.itemConfiguration.GetMaxAmount());
        }
        return isSuccess;
    }
    public int GetCurrentCapacity()
    {
        int currentCapacity = 0;
        int addedSlot = 0;
        currentCapacity = inventoryList.Count;
        foreach (KeyValuePair<ItemId, Inventory> inventory in inventoryList)
        {
            addedSlot = (int)Mathf.Floor((inventory.Value.GetItemAmount() + 1) / inventory.Value.item.itemConfiguration.GetMaxAmount());
        }
        return currentCapacity + addedSlot;
    }

    public void DropItem(ItemId itemId, int amount)
    {
        int inventoryRemain = inventoryList[itemId].GetItemAmount() - amount;

        inventoryList[itemId].SetAmount(inventoryRemain);
        if (inventoryRemain <= 0) inventoryList.Remove(itemId);

        // have to instantiate
    }
    public bool UseItem(ItemId selectedItem)
    {
        return true;
    }
    public Dictionary<ItemId, Inventory> GetInventory()
    {
        return inventoryList;
    }

#if UNITY_EDITOR
    private void OnInventoryOpen()
    {
        GlobalEventSystem.OpenInventory();
    }
#endif
    private void OnDestroy()
    {
#if UNITY_EDITOR
        playerInput.Inventory -= OnInventoryOpen;
#endif
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.TryGetComponent<DropItem>(out DropItem item);

        if (!item) return;

        ItemId itemId = item.GetItemId();
        int amount = item.GetAmount();
        Sprite itemSprite = item.itemConfiguration.GetItemDropSprite();
        switch (itemId)
        {
            case ItemId.Gold:
                currencyManager.AddAmount(amount);
                break;
            default:
                AddItem(item, amount);
                break;
        }

        item.PickUp();
        itemPickUpFader.ShowGetItem(itemSprite);
    }
}

[Serializable]
public class Inventory
{
    public BaseItem item;
    private int itemAmount;
    public Inventory() { }
    public Inventory(BaseItem item, int itemAmount)
    {
        this.item = item;
        this.itemAmount = itemAmount;
    }

    public int GetItemAmount()
    {
        return itemAmount;
    }

    public void AddAmount(int amount)
    {
        itemAmount += amount;
    }
    public void SetAmount(int amount)
    {
        itemAmount = amount;
    }
}
