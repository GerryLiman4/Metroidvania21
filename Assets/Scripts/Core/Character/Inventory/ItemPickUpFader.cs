using UnityEngine;

public class ItemPickUpFader : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public void ShowGetItem(Sprite item)
    {
        spriteRenderer.sprite = item;
        animator.gameObject.SetActive(true);
        animator.enabled = true;
    }
    public void HideGetItem()
    {
        animator.enabled = false;
        animator.gameObject.SetActive(false);
    }
}
