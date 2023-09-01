using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [SerializeField] private List<IInteractable> interactables = new List<IInteractable>();
    [SerializeField] private InventoryManager inventoryManager;

#if UNITY_EDITOR
    [SerializeField] private PlayerInputManager playerInput;
#endif
    private void Awake()
    {
#if UNITY_EDITOR
        playerInput.Interact += Interact;
#endif
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            if (!CanShowInteractBubble(interactable)) return;
            interactable.ShowInteractBubble();
            interactables.Add(interactable);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            if (!CanShowInteractBubble(interactable)) return;
            interactable.HideInteractBubble();
            interactables.Remove(interactable);
        }
    }
    public void Interact()
    {
        if (interactables.Count <= 0) return;
        IInteractable nearestInteraction = interactables[0];
        if (interactables.Count == 1)
        {
            if (!CanInteract(nearestInteraction)) return;
            nearestInteraction.Interact();
        }
        else
        {
            float nearestDistance = Mathf.Infinity;

            foreach (IInteractable interactable in interactables)
            {
                float distanceToPlayer = Vector3.Distance(interactable.GetPosition(), transform.position);
                if (distanceToPlayer < nearestDistance)
                {
                    nearestInteraction = interactable;
                    nearestDistance = distanceToPlayer;
                }
            }
            if (!CanInteract(nearestInteraction)) return;
            nearestInteraction.Interact();
        }
    }
    private bool CanShowInteractBubble(IInteractable interactable)
    {
        return interactable.HasInteraction();
    }

    private bool CanInteract(IInteractable interactable)
    {
        ItemId requiredItem = interactable.GetItemRequirement();
        if (requiredItem == ItemId.None) return true;

        return inventoryManager.CheckItemExist(requiredItem);
    }
    private void OnDestroy()
    {
#if UNITY_EDITOR
        playerInput.Interact -= Interact;
#endif
    }
}
