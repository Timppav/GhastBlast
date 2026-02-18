using UnityEngine;
using System.Collections;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup _gameOverPanelCG;
    [SerializeField] CanvasGroup _pauseMenuPanelCG;
    [SerializeField] CanvasGroup _levelUpPanelCG;
    [SerializeField] CanvasGroup _victoryPanelCG;
    [SerializeField] CanvasGroup _tripleShotTimerPanelCG;
    [SerializeField] float _levelUpButtonDelay = 1f;

    [Header("Audio")]
    [SerializeField] AudioClip _gameOverSound;
    [SerializeField] AudioClip _victorySound;

    CanvasGroup _cg;
    LevelUpPanel _levelUpPanel;
    TripleShotTimer _tripleShotTimer;
    bool _isPaused = false;
    bool _isLevelUpActive = false;
    
    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
        _levelUpPanel = GetComponentInChildren<LevelUpPanel>();
        _tripleShotTimer = GetComponentInChildren<TripleShotTimer>();
        _tripleShotTimer.OnTimerExpired += HideTripleShotTimerPanel;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isLevelUpActive)
            {
                return;
            }

            if (UIManager.Instance != null && UIManager.Instance.IsAnyPanelOpen())
            {
                return;
            }

            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }    
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

    public void ShowGameOverPanel()
    {
        CanvasGroupSetState(_gameOverPanelCG, true);
        if (_gameOverSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.StopCurrentMusic();
            AudioManager.Instance.PlayAudio(_gameOverSound, AudioManager.SoundType.SFX, 0.6f, false);
        }
    }

    public void ShowVictoryPanel()
    {
        CanvasGroupSetState(_victoryPanelCG, true);
        if (_victorySound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.StopCurrentMusic();
            AudioManager.Instance.PlayAudio(_victorySound, AudioManager.SoundType.SFX, 1f, false);
        }
    }

    public void ShowLevelUpPanel()
    {
        _isLevelUpActive = true;
        GameManager.Instance.PauseGame();
        StartCoroutine(ShowLevelUpPanelWithDelay());
    }

    IEnumerator ShowLevelUpPanelWithDelay() 
    {
        CanvasGroupSetState(_levelUpPanelCG, true);
        _levelUpPanel.DisableAllFrames();

        yield return new WaitForSecondsRealtime(_levelUpButtonDelay);

        _levelUpPanel.EnableAllFrames();
    }

    public void HideLevelUpPanel()
    {
        _isLevelUpActive = false;
        CanvasGroupSetState(_levelUpPanelCG, false);

        if (!_isPaused)
        {
            GameManager.Instance.ResumeGame();
        }
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
            CanvasGroupSetState(_pauseMenuPanelCG, true);
        });
    }

    public void Quit()
    {
        UIManager.Instance.Quit();
    }

    public void ShowTripleShotTimerPanel(float duration)
    {
        if (_tripleShotTimer.IsActive())
        {
            _tripleShotTimer.AddTime(duration);
        }
        else
        {
            CanvasGroupSetState(_tripleShotTimerPanelCG, true);
            _tripleShotTimer.StartTimer(duration);
        }
    }

    void HideTripleShotTimerPanel()
    {
        CanvasGroupSetState(_tripleShotTimerPanelCG, false);
    }
}
