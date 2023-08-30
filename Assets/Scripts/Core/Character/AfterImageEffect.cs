using System;
using UnityEngine;

public class AfterImageEffect : MonoBehaviour
{
    [SerializeField] private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;

    [SerializeField] private float alphaSet = 0.8f;
    private float alphaMultiplier = 0.85f;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private Transform playerTransform;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private float positionOffset = 0.5f;

    private Color color;
    public event Action<AfterImageEffect> Fade;
    public void Initialize(SpriteRenderer playerSpriteRenderer, Transform playerTransform)
    {
        this.playerTransform = playerTransform;
        this.playerSpriteRenderer = playerSpriteRenderer;
    }
    public void OnActivate()
    {
        alpha = alphaSet;
        spriteRenderer.sprite = playerSpriteRenderer.sprite;

        transform.position = playerTransform.position;
        transform.rotation = playerTransform.rotation;
        transform.localScale = playerTransform.localScale;
        //transform.localPosition = new Vector3(transform.localPosition.x - positionOffset, transform.localPosition.y, transform.localPosition.z);
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        spriteRenderer.color = color;

        if (Time.time >= (timeActivated + activeTime))
        {
            Fade?.Invoke(this);
        }
    }

    private void OnDestroy()
    {
        Fade = null;
    }
}
