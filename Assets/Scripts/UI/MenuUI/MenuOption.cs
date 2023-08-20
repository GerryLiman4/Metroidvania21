using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuOption : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler
{
    [SerializeField] private GameObject selectedIndicator;

    [SerializeField] private int optionIndex;

    public event Action<int> Highlighted;
    public event Action<int> Selected;
    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedIndicator.SetActive(true);
        Highlighted?.Invoke(optionIndex);
    }
    public void Initialize(int optionIndex)
    {
        this.optionIndex = optionIndex;
    }
    public void RemoveHighlight()
    {
        selectedIndicator.SetActive(false);
    }
    private void OnDestroy()
    {
        Highlighted = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedIndicator.SetActive(true);
        Selected?.Invoke(optionIndex);
    }
    public int GetOptionIndex()
    {
        return optionIndex;
    }
}
