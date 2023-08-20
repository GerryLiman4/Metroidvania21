using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LoadingScreenManager loadingScreenManager; 
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
        GlobalEventSystem.OnStartGame += OnStartGame;
    }

    private void OnStartGame()
    {
        loadingScreenManager.ShowLoadingScreen();
        StartCoroutine(LoadScene(1));
    }
    private IEnumerator LoadScene(int sceneIndex)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!loadingOperation.isDone)
        {
            yield return new WaitForSeconds(loadingScreenManager.GetMinLoadTime());
        }
        loadingScreenManager.HideLoadingScreen();
    }

    private void OnDestroy()
    {
        GlobalEventSystem.OnStartGame -= OnStartGame;
    }

}
