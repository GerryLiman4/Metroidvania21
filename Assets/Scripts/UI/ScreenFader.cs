using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image fader;

    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float fadeOutTime = 1f;
    [Range(0.05f, 0.2f)]
    [SerializeField] private float fadeInterval = 0.05f;

    private float fadeTimer = 0;
    private void Awake()
    {
        ScreenFader[] screenFaders = FindObjectsOfType<ScreenFader>();
        if (screenFaders.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    public IEnumerator FadeIn()
    {
        canvas.enabled = true;
        fadeTimer = 0;
        SetFaderAlpha(1f);

        while (fadeTimer < fadeInTime)
        {
            fadeTimer += fadeInterval;
            yield return new WaitForSecondsRealtime(fadeInterval);
            SetFaderAlpha(1f - (fadeTimer / fadeInTime));
        }
        canvas.enabled = false;
    }

    public IEnumerator FadeOut()
    {
        canvas.enabled = true;
        fadeTimer = 0;
        SetFaderAlpha(0f);

        while (fadeTimer < fadeInTime)
        {
            fadeTimer += fadeInterval;
            yield return new WaitForSecondsRealtime(fadeInterval);
            SetFaderAlpha(fadeTimer / fadeOutTime);
        }
        canvas.enabled = false;
    }
    private void SetFaderAlpha(float alpha)
    {
        var tempColor = fader.color;
        tempColor.a = alpha;
        fader.color = tempColor;
    }
}
