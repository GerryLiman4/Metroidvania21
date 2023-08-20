using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private Canvas loadingCanvas;

    [SerializeField] private TypeWriterText loadingText;
    [SerializeField] private Image gameIcon;
    [SerializeField] private GameObject background;
    [SerializeField] private float minimalLoadingTime = 1.25f;

    public static LoadingScreenManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            LoadingScreenManager[] loadingScreenManagers = FindObjectsOfType<LoadingScreenManager>();
            if(loadingScreenManagers.Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            instance = loadingScreenManagers[0];
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowLoadingScreen()
    {
        loadingCanvas.enabled = true;
        loadingText.gameObject.SetActive(true);
        gameIcon.gameObject.SetActive(true);
        background.gameObject.SetActive(true);

        loadingText.SetActive();
    }

    public void HideLoadingScreen()
    {
        // validasi pake timer min loading time
        loadingText.gameObject.SetActive(false);
        gameIcon.gameObject.SetActive(false);
        background.gameObject.SetActive(false);

        loadingCanvas.enabled = false;
    }

    public float GetMinLoadTime()
    {
        return minimalLoadingTime;
    }
}
