using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] CanvasGroup _mainMenuButtonsCG;
    CanvasGroup _mainMenuCG;

    void Awake()
    {
        _mainMenuCG = GetComponent<CanvasGroup>();
        OpenMainMenu();
    }

    void CanvasGroupSetState(CanvasGroup canvasGroup, bool state)
    {
        canvasGroup.alpha = state ? 1.0f : 0.0f;
        canvasGroup.interactable = state;
        canvasGroup.blocksRaycasts = state;
    }

    public void OpenMainMenu()
    {
        CanvasGroupSetState(_mainMenuCG, true);
    }

    public void CloseMainMenu()
    {
        CanvasGroupSetState(_mainMenuCG, false);
    }
    
    public void Play()
    {
        CloseMainMenu();
        GameManager.Instance.StartGame();
    }

    public void OpenSettings()
    {
        CanvasGroupSetState(_mainMenuButtonsCG, false);
        UIManager.Instance.OpenSettings(() => {
            CanvasGroupSetState(_mainMenuButtonsCG, true);
        });
    }

    public void OpenHelpMenu()
    {
        CanvasGroupSetState(_mainMenuButtonsCG, false);
        UIManager.Instance.OpenHelpMenu(() =>
        {
            CanvasGroupSetState(_mainMenuButtonsCG, true);
        });
    }

    public void OpenQuitConfirmation()
    {
        CanvasGroupSetState(_mainMenuButtonsCG, false);
        UIManager.Instance.OpenQuitConfirmation(() => {
            CanvasGroupSetState(_mainMenuButtonsCG, true);
        });
    }

    public void Quit()
    {
        UIManager.Instance.Quit();
    }
}
