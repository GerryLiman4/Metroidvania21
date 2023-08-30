using UnityEngine;

public class PlayerInformationUIManager : MonoBehaviour
{
    [SerializeField] private CurrencyUIManager currencyUIManager;
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private PlayerHealth healthManager;
    [SerializeField] private HealthUIManager healthUIManager;

    private int currentCurrencyAmount;
    private void Awake()
    {
        currencyManager.ChangeAmount += OnChangeAmount;
        currencyManager.SetCurrencyAmount += OnSetCurrencyAmount;

        healthManager.SetHealth += OnSetHealth;
        healthManager.Heal += OnHeal;
        healthManager.Damage += OnDamage;
    }

    private void OnDamage(int health)
    {
        healthUIManager.ReduceHealth(health);
    }

    private void OnHeal(int health)
    {
        healthUIManager.AddHealth(health);
    }

    private void OnSetHealth(int health)
    {
        healthUIManager.SetHealth(health);
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

        healthManager.SetHealth -= OnSetHealth;
        healthManager.Heal -= OnHeal;
        healthManager.Damage -= OnDamage;
    }
}
