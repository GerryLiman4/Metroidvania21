using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject leftPanel;
    [SerializeField] private GameObject rightPanel;

    [Tooltip("Left side inventory")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private List<InventorySlot> inventorySlots = new List<InventorySlot>();

    [Tooltip("Right side inventory")]
    [SerializeField] private TextMeshProUGUI selectedItemName;
    [SerializeField] private TextMeshProUGUI selectedItemDescription;
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;

    private Dictionary<ItemId, Inventory> inventoryList = new Dictionary<ItemId, Inventory>();
    private InventorySlot selectedItem;
    bool isOpen = false;
    private void Awake()
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            inventorySlot.Selected += OnSlotSelected;
        }
        GlobalEventSystem.OnOpenInventory += SetActive;
        useButton.onClick.AddListener(UseSelectedItem);
        dropButton.onClick.AddListener(DropSelectedItem);
    }

    private void DropSelectedItem()
    {
        Debug.Log(selectedItem);
        inventoryManager.DropItem(
            selectedItem.GetItemId(),
            selectedItem.GetItemAmount());
        selectedItem.gameObject.SetActive(false);
        Initialize();
    }

    private void UseSelectedItem()
    {
        inventoryManager.UseItem(selectedItem.GetItemId());
        // if true reinitialize
        Debug.Log("Use");
    }

    private void OnSlotSelected(InventorySlot slot)
    {
        selectedItem = slot;
        selectedItemName.text = inventoryList[selectedItem.GetItemId()].item.itemConfiguration.GetName();

        selectedItemDescription.text = inventoryList[selectedItem.GetItemId()].item.itemConfiguration.GetDescription();
    }

    public void SetActive()
    {
        isOpen = !isOpen;
        if (isOpen) Initialize();

        leftPanel.SetActive(isOpen);
        rightPanel.SetActive(isOpen);
    }

    public void Initialize()
    {
        ResetSelectedItem();
        ResetInventory();
        inventoryList = inventoryManager.GetInventory();

        int slotIndex = 0;

        foreach (KeyValuePair<ItemId, Inventory> inventory in inventoryList)
        {
            int itemAmount = inventory.Value.GetItemAmount();
            int itemMaxAmount = inventory.Value.item.itemConfiguration.GetMaxAmount();
            Sprite itemIcon = inventory.Value.item.itemConfiguration.GetItemIcon();
            ItemId itemId = inventory.Value.item.itemConfiguration.GetItemId();
            if (itemAmount > itemMaxAmount)
            {
                do
                {
                    inventorySlots[slotIndex].Initialize(itemIcon, itemMaxAmount, itemId);
                    itemAmount -= itemMaxAmount;
                    slotIndex++;
                } while (itemAmount > itemMaxAmount);

                inventorySlots[slotIndex].Initialize(itemIcon, itemAmount, itemId);
                slotIndex++;
            }
            else
            {
                inventorySlots[slotIndex].Initialize(itemIcon, itemAmount, itemId);
                slotIndex++;
            }
        }
    }
    public void ResetSelectedItem()
    {
        selectedItem = null;
        selectedItemName.text = "";

        selectedItemDescription.text = "";
    }

    public void ResetInventory()
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            inventorySlot.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            inventorySlot.Selected -= OnSlotSelected;
        }
        GlobalEventSystem.OnOpenInventory -= SetActive;

        useButton.onClick.RemoveAllListeners();
        dropButton.onClick.RemoveAllListeners();
    }
}
