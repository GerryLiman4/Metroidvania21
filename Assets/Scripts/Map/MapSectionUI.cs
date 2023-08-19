using UnityEngine;

public class MapSectionUI : MonoBehaviour
{
    [SerializeField] private MapId mapId;
    [SerializeField] private RectTransform mainCharacterIconRoot;
    [SerializeField] private GameObject checkPoint;

    [SerializeField] private bool isDiscovered = false;
    public MapId GetMapId()
    {
        return mapId;
    }
    public void MarkMapSection(GameObject mainCharacterIcon)
    {
        mainCharacterIcon.transform.SetParent(mainCharacterIconRoot);
        mainCharacterIcon.transform.localPosition = Vector3.zero;

        isDiscovered = true;
        if (checkPoint != null) checkPoint.SetActive(isDiscovered);
    }
    public void Initialize(bool isDiscovered)
    {
        this.isDiscovered = isDiscovered;

        if (checkPoint != null) checkPoint.SetActive(isDiscovered);
    }
}
