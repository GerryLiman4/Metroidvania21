using UnityEngine;

public class MapDetector : MonoBehaviour
{
    [SerializeField] private LayerMask mapLayer;
    [SerializeField] private MapId currentMapId;

#if UNITY_EDITOR
    [SerializeField] private PlayerMove playerMovement;
    [SerializeField] private PlayerInputManager playerInput;
#endif
    private const float raycastRange = 0.1f;
    private void Awake()
    {
#if UNITY_EDITOR
        playerInput.Map += OnOpenMap;
#endif
    }

#if UNITY_EDITOR
    private void OnOpenMap()
    {
        GlobalEventSystem.OpenMap();
    }
    private void LateUpdate()
    {
        Vector2 moveDirection = playerMovement.GetMoveDirection();
        if (moveDirection.x != 0 || moveDirection.y != 0) CheckMap();
    }
#endif
    public void CheckMap()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, raycastRange, layerMask: mapLayer);
        if (!hit) return;

        hit.collider.TryGetComponent<MapSection>(out MapSection mapSection);
        MapId newMapId = mapSection.GetMapId();

        if (currentMapId == newMapId) return;

        currentMapId = newMapId;
        GlobalEventSystem.ChangeMap(newMapId);
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        playerInput.Map -= OnOpenMap;
#endif
    }
}
