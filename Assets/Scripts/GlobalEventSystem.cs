using System;

public static class GlobalEventSystem 
{
    public static event Action OnOpenInventory;
    public static event Action OnOpenMap;

    public static event Action<MapId> OnChangeMap;
    public static void OpenInventory()
    {
        OnOpenInventory?.Invoke();
    }
    public static void OpenMap()
    {
        OnOpenMap?.Invoke();
    }
    public static void ChangeMap(MapId mapId)
    {
        OnChangeMap?.Invoke(mapId);
    }
    public static void UnsubscribeAll()
    {
        OnOpenInventory = null;
        OnOpenMap = null;
        OnChangeMap = null;
    }

}
