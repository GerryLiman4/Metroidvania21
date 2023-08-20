using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriterText : MonoBehaviour
{
    [TextArea]
    [SerializeField] private string text;
    [SerializeField] private TextMeshProUGUI uiText;

    [SerializeField] private float typingTime;
    [SerializeField] private bool isLooping;
    private string currentText;
    private IEnumerator typing;

    public void SetActive()
    {
        currentText = "";
        uiText.text = "";
        Debug.Log("Active");
        typing = TypeWriting();

        StartCoroutine(typing);
    }

    private IEnumerator TypeWriting()
    {
        foreach (char c in text)
        {
            currentText += c;
            uiText.text = currentText;
            yield return new WaitForSeconds(typingTime);
        }

        if (isLooping) SetActive();
    }

}
