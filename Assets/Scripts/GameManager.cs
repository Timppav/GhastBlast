using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    InGameUIManager _inGameUIManager;
    [SerializeField] GameObject _loadingScreen;
    [SerializeField] Slider _loadingBar;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TestLevel")
        {
            _inGameUIManager = FindFirstObjectByType<InGameUIManager>();
            _inGameUIManager?.ShowInGameUI();
        }
    }

    public void StartGame()
    {
        StartCoroutine(LoadSceneAsync("TestLevel"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        if (_loadingScreen != null)
        {
            _loadingScreen.SetActive(true);
        }

        if (_loadingBar != null)
        {
            _loadingBar.value = 0f;
        }

        float minimumLoadtime = 0.5f;
        float startTime = Time.time;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float displayProgress = 0f;

        // Loading bar animation
        while (displayProgress < 1f)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            displayProgress = Mathf.MoveTowards(displayProgress, targetProgress, Time.deltaTime * 2f);
            
            if (_loadingBar != null)
                _loadingBar.value = displayProgress;
            
            yield return null;
        }

        if (_loadingBar != null)
        {
            _loadingBar.value = 1f;
        }

        // Artifical loading screen delay
        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minimumLoadtime)
        {
            yield return new WaitForSeconds(minimumLoadtime - elapsedTime);
        }

        operation.allowSceneActivation = true;

        // Wait one frame for scene to actually load
        yield return null;

        Time.timeScale = 1f;

        if (_loadingScreen != null)
        {
            _loadingScreen.SetActive(false);
        }
    }

    public void ResetGame()
    {
        Time.timeScale = 0f;
        _inGameUIManager = null;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        _inGameUIManager.ShowGameOverPanel();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}