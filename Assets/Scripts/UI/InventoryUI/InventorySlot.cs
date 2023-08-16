using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI amountText;

    private ItemId itemId;
    private int itemAmount;

    public event Action<InventorySlot> Selected;
    public void Initialize(Sprite itemIcon, int amount, ItemId itemId)
    {
        this.itemIcon.sprite = itemIcon;
        this.itemId = itemId;

        itemAmount = amount;
        amountText.text = amount + "x";
        gameObject.SetActive(true);
    }

    public ItemId GetItemId()
    {
        return itemId;
    }
    public int GetItemAmount()
    {
        return itemAmount;
    }

    private void OnDestroy()
    {
        Selected = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Selected?.Invoke(this);
    }
}
