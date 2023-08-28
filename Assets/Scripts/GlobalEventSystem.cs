using System;

public static class GlobalEventSystem
{
    // InGame
    public static event Action OnOpenInventory;
    public static event Action OnOpenMap;
    public static event Action OnInteract;

    public static event Action<MapId> OnChangeMap;
    // Scene
    public static event Action OnGoToMainMenu;
    public static event Action OnStartGame;

    public static GameSetting settingInstance;
    #region InGame
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
    public static void Interact()
    {
        OnInteract?.Invoke();
    }
    #endregion

    #region Menu
    public static void GoToMainMenu()
    {
        OnGoToMainMenu?.Invoke();
    }
    public static void StartGame()
    {
        OnStartGame?.Invoke();
    }
    #endregion
    public static void UnsubscribeAll()
    {
        OnOpenInventory = null;
        OnOpenMap = null;
        OnChangeMap = null;
        OnGoToMainMenu = null;
    }
    public static void InitializeSetting()
    {
        if(settingInstance == null)
        {
            settingInstance = new GameSetting();
        }
    }
}
