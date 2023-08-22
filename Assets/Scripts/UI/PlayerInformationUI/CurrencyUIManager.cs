using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private Image currencyIcon;

    [SerializeField] private Color plusColor = Color.green;
    [SerializeField] private Color deductedColor = Color.red;
    [Range(0.05f, 0.3f)]
    [SerializeField] private float coloredInterval = 0.2f;
    private int currentCurrencyAmount;
    private Color originalTextColor;
    private void Awake()
    {
        originalTextColor = currencyText.color;
    }
    public void SetCurrencyAmount(int amount, bool isPlus)
    {
        ResetColor();
        currentCurrencyAmount = amount;
        currencyText.text = currentCurrencyAmount.ToString();
        StartCoroutine(ChangeColor(isPlus));
    }
    public void SetCurrencyAmount(int amount)
    {
        ResetColor();
        currentCurrencyAmount = amount;
        currencyText.text = currentCurrencyAmount.ToString();
    }
    private IEnumerator ChangeColor(bool isPlus)
    {
        currencyText.color = isPlus ? plusColor : deductedColor;
        yield return new WaitForSecondsRealtime(coloredInterval);
        ResetColor();
    }
    private void ResetColor() 
    {
        currencyText.color = originalTextColor;
    }
}
