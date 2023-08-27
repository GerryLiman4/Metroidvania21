using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float maxItemBurstSpeed = 3f;

    [SerializeField] private bool isBurst = false;
    [SerializeField] private DropItem itemPrefab;

    [SerializeField] private Vector2 xBurstRange = new Vector2(-0.8f, 0.8f);
    [SerializeField] private Vector2 burstPower = new Vector2(200f,500f);
    public void SetItemPrefab(ItemConfiguration item, int itemAmount)
    {
        itemPrefab.itemConfiguration = item;
        itemPrefab.SetAmount(itemAmount);
    }
    public void Spawn()
    {
        DropItem dropItem = Instantiate(itemPrefab, position: transform.position, Quaternion.identity);
        if (!isBurst) return;
        Vector2 direction = new Vector2(Random.Range(xBurstRange.x,xBurstRange.y), 1f);
        dropItem.DropPush(direction, Random.Range(burstPower.x, burstPower.y));
    }
}
