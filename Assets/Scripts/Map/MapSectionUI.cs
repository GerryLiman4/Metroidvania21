using UnityEngine;

public class MapSectionUI : MonoBehaviour
{
    [SerializeField] private MapId mapId;
    [SerializeField] private RectTransform mainCharacterIconRoot;
    [SerializeField] private RectTransform checkPointIconRoot;

    [SerializeField] private bool isDiscovered = false;
    public MapId GetMapId()
    {
        return mapId;
    }
    public void MarkMapSection(GameObject mainCharacterIcon)
    {
        mainCharacterIcon.transform.SetParent(mainCharacterIconRoot);
        mainCharacterIcon.transform.localPosition = Vector3.zero;

        if (isDiscovered) return;
    }
}
