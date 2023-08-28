using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private int maxAmount;

    private int currentCurrencyAmount;
    public event Action<int, bool> ChangeAmount;
    public event Action<int> SetCurrencyAmount;
#if UNITY_EDITOR
    private int testAmount = 20;
    private void Start()
    {
        SetAmount(testAmount);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddAmount(5);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ReduceAmount(5);
        }
    }
#endif
    public void AddAmount(int amount)
    {
        currentCurrencyAmount = Mathf.Clamp(currentCurrencyAmount + amount, 0, maxAmount);
        ChangeAmount?.Invoke(currentCurrencyAmount, true);
    }
    public void ReduceAmount(int amount)
    {
        currentCurrencyAmount = Mathf.Clamp(currentCurrencyAmount - amount, 0, maxAmount);
        ChangeAmount?.Invoke(currentCurrencyAmount, false);
    }
    public void SetAmount(int amount)
    {
        currentCurrencyAmount = Mathf.Clamp(currentCurrencyAmount - amount, 0, maxAmount);
        SetCurrencyAmount?.Invoke(currentCurrencyAmount);
    }
    private void OnDestroy()
    {
        ChangeAmount = null;
        SetCurrencyAmount = null;
    }
}
