using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] Image _expBarFill;
    [SerializeField] Player _player;
    [SerializeField] TextMeshProUGUI _expText;
    [SerializeField] TextMeshProUGUI _levelText;

    float _currentExp;
    float _nextLevelExp;
    float _currentLevel;

    void Start()
    {
        if (_player != null)
        {
            _currentExp = _player.GetPlayerExp();
            _nextLevelExp = _player.GetNextLevelExp();
            _currentLevel = _player.GetPlayerLevel();
            UpdateExpDisplay();
            UpdateLevelDisplay();
        }
    }

    void OnEnable()
    {
        if (_player != null)
        {
            _player.OnExpChanged += OnExpChanged;
            _player.OnLevelUp += OnLevelUp;
        }
    }

    void OnDisable()
    {
        if (_player != null)
        {
            _player.OnExpChanged -= OnExpChanged;
            _player.OnLevelUp -= OnLevelUp;
        }
    }

    void OnExpChanged(float currentExp, float nextLevelExp)
    {
        _currentExp = currentExp;
        _nextLevelExp = nextLevelExp;
        UpdateExpDisplay();
    }

    void OnLevelUp(float newLevel)
    {
        _currentLevel = newLevel;
        UpdateLevelDisplay();
    }

    void UpdateExpDisplay()
    {
        if (_expBarFill != null)
        {
            _expBarFill.fillAmount = _currentExp / _nextLevelExp;
        }

        if (_expText != null)
        {
            _expText.text = $"EXP: {_currentExp:F0}/{_nextLevelExp:F0}";
        }
    }

    void UpdateLevelDisplay()
    {
        if (_levelText != null)
        {
            _levelText.text = $"Level {_currentLevel}";
        }
    }
}
