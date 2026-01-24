using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup _gameOverPanelCG;
    [SerializeField] CanvasGroup _pauseMenuPanelCG;

    CanvasGroup _cg;
    bool _isPaused = false;

    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void CanvasGroupSetState(CanvasGroup canvasGroup, bool state)
    {
        canvasGroup.alpha = state ? 1.0f : 0.0f;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    public void ShowInGameUI()
    {
        CanvasGroupSetState(_cg, true);
    }

    public void PauseGame()
    {
        _isPaused = true;
        GameManager.Instance.PauseGame();
        ShowPauseMenuPanel();
    }

    public void ResumeGame()
    {
        _isPaused = false;
        GameManager.Instance.ResumeGame();
        HidePauseMenuPanel();
    }

    void ShowPauseMenuPanel()
    {
        CanvasGroupSetState(_pauseMenuPanelCG, true);
    }

    void HidePauseMenuPanel()
    {
        CanvasGroupSetState(_pauseMenuPanelCG, false);
    }

    public void ShowGameOverPanel()
    {
        CanvasGroupSetState(_gameOverPanelCG, true);
    }

    public void ReturnToMainmenu()
    {
        UIManager.Instance.ReturnToMainMenu();
    }

    public void OpenSettings()
    {
        CanvasGroupSetState(_pauseMenuPanelCG, false);
        UIManager.Instance.OpenSettings(() => {
            CanvasGroupSetState(_pauseMenuPanelCG, true);
        });
    }

    public void OpenMainMenuConfirmation()
    {
        CanvasGroupSetState(_pauseMenuPanelCG, false);
        UIManager.Instance.OpenMainMenuConfirmation(() => {
            CanvasGroupSetState(_pauseMenuPanelCG, true);
        });
    }

    public void OpenQuitConfirmation()
    {
        CanvasGroupSetState(_pauseMenuPanelCG, false);
        UIManager.Instance.OpenQuitConfirmation(() => {
            // This runs when quit confirmation closes
            CanvasGroupSetState(_pauseMenuPanelCG, true);
        });
    }

    public void Quit()
    {
        UIManager.Instance.Quit();
    }
}
