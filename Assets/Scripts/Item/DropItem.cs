using UnityEngine;

public class DropItem : BaseItem
{
    [SerializeField] private bool canExpired;
    [SerializeField] private bool isFloating;
    [SerializeField] private bool isRotating;
    [SerializeField] private bool isStatic = false;

    [Range(0.1f, 2f)]
    [SerializeField] private float rotatingSpeed = 0.1f;
    [SerializeField] private int amount;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D selfCollider;
    [SerializeField] private Rigidbody2D rigidBody;
    private void Awake()
    {
        Initialize();
    }
    private void Update()
    {
        if (isRotating)
        {
            Rotate();
        }
    }
    private void Rotate()
    {
        transform.Rotate(0f, rotatingSpeed, 0f, Space.World);
    }
    private void Float()
    {

    }
    public void Initialize()
    {
        spriteRenderer.sprite = itemConfiguration.GetItemDropSprite();

        SetFreezePosition();
    }
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
    public void SetStatic(bool isStatic)
    {
        this.isStatic = isStatic;

        SetFreezePosition();
    }
    private void SetFreezePosition()
    {
        if (isStatic)
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        else
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

}
