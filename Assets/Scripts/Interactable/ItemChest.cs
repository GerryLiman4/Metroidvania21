using System.Collections.Generic;
using UnityEngine;

public class ItemChest : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ItemConfiguration> itemList = new List<ItemConfiguration>();
    [SerializeField] private List<int> itemAmountList = new List<int>();
    [SerializeField] private bool hasItemRequirement = false;
    [SerializeField] private ItemId requiredItem;

    [SerializeField] private ItemSpawner itemSpawner;

    [SerializeField] private SpriteRenderer interactBubbleRenderer;
    [SerializeField] private Animator interactBubbleAnimator;
    private bool hasOpened = false;
    public ItemId GetItemRequirement()
    {
        return requiredItem;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool HasInteraction()
    {
        if (hasOpened) return false;
        return true;
    }

    public void HideInteractBubble()
    {
        interactBubbleRenderer.gameObject.SetActive(false);
        interactBubbleRenderer.enabled = false;
        interactBubbleAnimator.enabled = false;
    }

    public void Interact(params object[] objects)
    {
        if ((hasItemRequirement && (objects.Length <= 0 || objects[0] is not ItemId)) || hasOpened) return;
        if (itemList.Count > 0)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                itemSpawner.SetItemPrefab(itemList[i], itemAmountList[i]);
                itemSpawner.Spawn();
            }
        }
        hasOpened = true;
        HideInteractBubble();
    }

    public void ShowInteractBubble()
    {
        if (hasOpened) return;
        interactBubbleRenderer.gameObject.SetActive(true);
        interactBubbleRenderer.enabled = true;
        interactBubbleAnimator.enabled = true;
    }
}
