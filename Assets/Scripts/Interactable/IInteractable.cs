using UnityEngine;

public interface IInteractable
{
    public void Interact(params object[] objects);
    public ItemId GetItemRequirement();
    public void ShowInteractBubble();
    public void HideInteractBubble();
    public Vector3 GetPosition();
    public bool HasInteraction();
}
