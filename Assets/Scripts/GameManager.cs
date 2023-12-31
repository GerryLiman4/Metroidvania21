using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LoadingScreenManager loadingScreenManager;
    [SerializeField] private ScreenFader screenFader;
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            GameManager[] sceneManagers = FindObjectsOfType<GameManager>();
            if (sceneManagers.Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            instance = sceneManagers[0];
            DontDestroyOnLoad(gameObject);
        }
        if(loadingScreenManager == null)
        {
            loadingScreenManager = FindObjectOfType<LoadingScreenManager>();
        }
        if(screenFader == null)
        {
            screenFader = FindObjectOfType<ScreenFader>();
        }

        GlobalEventSystem.OnStartGame += OnStartGame;
        GlobalEventSystem.InitializeSetting();
    }

    private void OnStartGame()
    {
        StartCoroutine(LoadScene((int)SceneId.InGame));
    }
    private IEnumerator LoadScene(int sceneIndex)
    {
        yield return StartCoroutine(screenFader.FadeOut());

        loadingScreenManager.ShowLoadingScreen();

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!loadingOperation.isDone)
        {
            yield return new WaitForSecondsRealtime(loadingScreenManager.GetMinLoadTime());
        }
        InitializeGame();
        loadingScreenManager.HideLoadingScreen();

        yield return StartCoroutine(screenFader.FadeIn());
    }
    private void InitializeGame()
    {
        CurrencyManager playerCurrency = FindObjectOfType<CurrencyManager>();
        if(playerCurrency != null)
        {
            // placeholder should get from save file
            playerCurrency.SetAmount(0);
        }
    }

    private void OnDestroy()
    {
        GlobalEventSystem.OnStartGame -= OnStartGame;
    }

}
