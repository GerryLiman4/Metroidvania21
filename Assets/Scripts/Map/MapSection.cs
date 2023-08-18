using UnityEngine;

public class MapSection : MonoBehaviour
{
    [SerializeField] private MapId mapId;
    
    public MapId GetMapId()
    {
        return mapId;
    }
}
