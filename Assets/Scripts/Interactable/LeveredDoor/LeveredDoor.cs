using System.Collections;
using UnityEngine;

public class LeveredDoor : MonoBehaviour,ILevered
{
    [SerializeField] private Transform OpenPositionRoot;
    [SerializeField] private Transform ClosePositionRoot;

    [SerializeField] private Vector2 OpenPosition;
    [SerializeField] private Vector2 ClosePosition;
    [SerializeField] private bool isOpen = false;
    [Range(0.05f,0.1f)]
    [SerializeField] private float animationInterval = 0.05f;
    [SerializeField] private float animationTime = 3f;

    private void Awake()
    {
        ClosePosition = ClosePositionRoot.position;
        OpenPosition = OpenPositionRoot.position;
    }
    public void OffInteraction()
    {
        if (!isOpen) return;
        StartCoroutine(CloseDoor());
    }

    public void OnInteraction()
    {
        if (isOpen) return;
        StartCoroutine(OpenDoor());
    }
    private IEnumerator OpenDoor()
    {
        float fractionOfJourney = 0f;
        float animationTimer = 0f;

        while (fractionOfJourney < 1)
        {
            transform.position = Vector3.Lerp(ClosePosition, OpenPosition, fractionOfJourney);
            yield return new WaitForSeconds(animationInterval);
            animationTimer += animationInterval;
            fractionOfJourney = animationTimer / animationTime;
        }
        isOpen = true;
    }
    private IEnumerator CloseDoor()
    {
        float fractionOfJourney = 0f;
        float animationTimer = 0f;

        while (fractionOfJourney < 1)
        {
            transform.position = Vector3.Lerp(OpenPosition, ClosePosition, fractionOfJourney);
            yield return new WaitForSeconds(animationInterval);
            animationTimer += animationInterval;
            fractionOfJourney = animationTimer / animationTime;
        }
        isOpen = false;
    }
}
