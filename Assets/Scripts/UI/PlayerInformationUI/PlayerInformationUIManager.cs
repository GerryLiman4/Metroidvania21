using System;
using UnityEngine;

public class PlayerInformationUIManager : MonoBehaviour
{
    [SerializeField] private CurrencyUIManager currencyUIManager;
    [SerializeField] private CurrencyManager currencyManager;

    private int currentCurrencyAmount;
    private void Awake()
    {
        currencyManager.ChangeAmount += OnChangeAmount;
        currencyManager.SetCurrencyAmount += OnSetCurrencyAmount;
    }

    private void OnSetCurrencyAmount(int amount)
    {
        currencyUIManager.SetCurrencyAmount(amount);
    }

    private void OnChangeAmount(int amount, bool isPlus)
    {
        currencyUIManager.SetCurrencyAmount(amount, isPlus);
    }
    private void OnDestroy()
    {
        currencyManager.ChangeAmount -= OnChangeAmount;
        currencyManager.SetCurrencyAmount -= OnSetCurrencyAmount;
    }
}
