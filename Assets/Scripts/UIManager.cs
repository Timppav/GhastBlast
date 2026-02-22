using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] CanvasGroup _settingsMenuCG;
    [SerializeField] CanvasGroup _quitConfirmationCG;
    [SerializeField] CanvasGroup _mainMenuConfirmationCG;
    [SerializeField] CanvasGroup _helpMenuCG;

    System.Action _onSettingsClosed;
    System.Action _onQuitConfirmationClosed;
    System.Action _onMainMenuConfirmationClosed;
    System.Action _onHelpMenuClosed;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void CanvasGroupSetState(CanvasGroup canvasGroup, bool state)
    {
        canvasGroup.alpha = state ? 1.0f : 0.0f;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    public bool IsAnyPanelOpen()
    {
        return _settingsMenuCG.alpha > 0 || _mainMenuConfirmationCG.alpha > 0 || _quitConfirmationCG.alpha > 0 || _helpMenuCG.alpha > 0;
    }

    public void OpenSettings(System.Action onClose = null)
    {
        _onSettingsClosed = onClose;
        CanvasGroupSetState(_settingsMenuCG, true);
    }

    public void CloseSettings()
    {
        CanvasGroupSetState(_settingsMenuCG, false);
        _onSettingsClosed?.Invoke();
        _onSettingsClosed = null;
    }

    public void OpenHelpMenu(System.Action onClose = null)
    {
        _onHelpMenuClosed = onClose;
        CanvasGroupSetState(_helpMenuCG, true);
    }

    public void CloseHelpMenu()
    {
        CanvasGroupSetState(_helpMenuCG, false);
        _onHelpMenuClosed?.Invoke();
        _onHelpMenuClosed = null;
    }

    public void OpenMainMenuConfirmation(System.Action onClose = null)
    {
        _onMainMenuConfirmationClosed = onClose;
        CanvasGroupSetState(_mainMenuConfirmationCG, true);
    }

    public void CloseMainMenuConfirmation()
    {
        CanvasGroupSetState(_mainMenuConfirmationCG, false);
        _onMainMenuConfirmationClosed?.Invoke();
        _onMainMenuConfirmationClosed = null;
    }

    public void ReturnToMainMenu() {
        CloseMainMenuConfirmation();
        GameManager.Instance.ResetGame();
    }

    public void OpenQuitConfirmation(System.Action onClose = null)
    {
        _onQuitConfirmationClosed = onClose;
        CanvasGroupSetState(_quitConfirmationCG, true);
    }

    public void CloseQuitConfirmation()
    {
        CanvasGroupSetState(_quitConfirmationCG, false);
        _onQuitConfirmationClosed?.Invoke();
        _onQuitConfirmationClosed = null;
    }

    public void Quit()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
