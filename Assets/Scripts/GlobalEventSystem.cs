using System;

public static class GlobalEventSystem 
{
    public static event Action OnOpenInventory;

    public static void OpenInventory()
    {
        OnOpenInventory?.Invoke();
    }

    public static void UnsubscribeAll()
    {
        OnOpenInventory = null;
    }
}
