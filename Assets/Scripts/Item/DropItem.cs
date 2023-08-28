using UnityEngine;

public class DropItem : BaseItem
{
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
        if (itemConfiguration.isRotating)
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
    public void SetAmount(int amount)
    {
        this.amount = amount;
    }

    public void PickUp()
    {
        Destroy(gameObject);
    }
    public void SetStatic(bool isStatic)
    {
        itemConfiguration.isStatic = isStatic;

        SetFreezePosition();
    }
    private void SetFreezePosition()
    {
        if (itemConfiguration.isStatic)
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        else
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void DropPush(Vector2 direction, float pushPower)
    {
        rigidBody.AddForce(direction * pushPower,ForceMode2D.Force);
    }
}
