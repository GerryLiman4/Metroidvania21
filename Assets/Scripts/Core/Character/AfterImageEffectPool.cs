using System.Collections.Generic;
using UnityEngine;

public class AfterImageEffectPool : MonoBehaviour
{
    [SerializeField] private AfterImageEffect afterImagePrefab;
    [SerializeField] private int numberOfPool = 10;
    private Queue<AfterImageEffect> availableObjects = new Queue<AfterImageEffect>();

    private Transform playerTransform;
    private SpriteRenderer playerSpriteRenderer;
    public void Initialize(SpriteRenderer playerSpriteRenderer, Transform playerTransform)
    {
        this.playerTransform = playerTransform;
        this.playerSpriteRenderer = playerSpriteRenderer;
    }
    private void GrowPool()
    {
        for (int i = 0; i < numberOfPool; i++)
        {
            AfterImageEffect instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.Initialize(playerSpriteRenderer, playerTransform);
            instanceToAdd.transform.SetParent(transform);
            instanceToAdd.Fade += AddToPool;
            AddToPool(instanceToAdd);
        }
    }
    public void AddToPool(AfterImageEffect instance)
    {
        instance.gameObject.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    public AfterImageEffect GetFromPool()
    {
        if (availableObjects.Count == 0)
        {
            GrowPool();
        }
        var instance = availableObjects.Dequeue();
        instance.OnActivate();
        instance.gameObject.SetActive(true);
        return instance;
    }
    private void OnDestroy()
    {
        foreach (AfterImageEffect availableObject in availableObjects)
        {
            availableObject.Fade -= AddToPool;
        }
        availableObjects.Clear();
    }
}
