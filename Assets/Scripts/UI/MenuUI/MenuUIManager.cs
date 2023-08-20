using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] List<MenuOption> optionList = new List<MenuOption>();

    private int selectedIndex;
    public event Action<int> Selected;
    private void Awake()
    {
        foreach (MenuOption option in optionList)
        {
            option.Highlighted += OnHighlighted;
            option.Selected += OnSelected;
        }
    }

    private void OnSelected(int optionIndex)
    {
        selectedIndex = optionIndex;
        Selected?.Invoke(selectedIndex);
        RemoveDeselectedHighlight();
    }

    private void OnHighlighted(int optionIndex)
    {
        selectedIndex = optionIndex;
        RemoveDeselectedHighlight();
    }
    private void RemoveDeselectedHighlight()
    {
        foreach (MenuOption option in optionList)
        {
            if (option.GetOptionIndex() == selectedIndex) continue;
            option.RemoveHighlight();
        }
    }

    public void Show()
    {
        selectedIndex = -1;
        foreach (MenuOption option in optionList)
        {
            option.gameObject.SetActive(true);
            option.RemoveHighlight();
        }
    }
    public void Hide()
    {
        foreach (MenuOption option in optionList)
        {
            option.gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        foreach (MenuOption option in optionList)
        {
            option.Highlighted -= OnHighlighted;
            option.Selected -= OnSelected;
        }
    }
}
