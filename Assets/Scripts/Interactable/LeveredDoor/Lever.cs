using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isActive = false;
    [SerializeField] private bool isTwoWay = false;
    [SerializeField] private List<GameObject> pairedObjects = new List<GameObject>();
    [SerializeField] private List<ILevered> pairedInterfaces = new List<ILevered>();

    [SerializeField] private bool hasItemRequirement = false;
    [SerializeField] private ItemId requiredItem;

    [SerializeField] private SpriteRenderer interactBubbleRenderer;
    [SerializeField] private Animator interactBubbleAnimator;
    private void Awake()
    {
        foreach (GameObject pairedObject in pairedObjects)
        {
            pairedObject.TryGetComponent<ILevered>(out ILevered pairedInterface);
            if (pairedInterface != null) pairedInterfaces.Add(pairedInterface);
        }
    }
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
        if (!isTwoWay && isActive) return false;
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
        if ((hasItemRequirement && (objects.Length <= 0 || objects[0] is not ItemId)) || (!isTwoWay && isActive)) return;
        foreach (ILevered pairedInterface in pairedInterfaces)
        {
            isActive = !isActive;
            if (isActive)
            {
                pairedInterface.OnInteraction();
            }
            else
            {
                pairedInterface.OffInteraction();
            }
        }
        if (isTwoWay) return;
        HideInteractBubble();
    }

    public void ShowInteractBubble()
    {
        if (!isTwoWay && isActive) return;
        interactBubbleRenderer.gameObject.SetActive(true);
        interactBubbleRenderer.enabled = true;
        interactBubbleAnimator.enabled = true;
    }
    private void OnDestroy()
    {
        pairedObjects.Clear();
        pairedInterfaces.Clear();
    }
}
