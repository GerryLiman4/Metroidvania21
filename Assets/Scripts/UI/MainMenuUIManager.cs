using System;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private MenuUIManager menuUIManager;

    private void Awake()
    {
        menuUIManager.Selected += OnSelected;
    }
    private void OnSelected(int selectedIndex)
    {
        menuUIManager.Hide();
        switch (selectedIndex)
        {
            case 0:
                GlobalEventSystem.StartGame();
                menuUIManager.Hide();
                break;
            case 1:
                break;
            case 2:
                Application.Quit();
                break;
            default:
                break;
        }
    }

    public void Show()
    {
        menuUIManager.Show();
    }
    public void Hide()
    {
        menuUIManager.Hide();
    }
}
