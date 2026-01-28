using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] bool _isPlayer = false;
    [SerializeField] EntityHealth _entityHealth;
    [SerializeField] Image _hpBarFill;
    [SerializeField] TextMeshProUGUI _healthText;

    float _currentHealth;
    float _maxHealth;

    void Awake()
    {
        if (_isPlayer)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _entityHealth = playerObj.GetComponent<EntityHealth>();
            }
        }

        if (_entityHealth != null)
        {
            _currentHealth = _entityHealth.GetCurrentHealth();
            _maxHealth = _entityHealth.GetMaxHealth();
            UpdateHealthText();
        }
    }

    void OnEnable()
    {
        if (_entityHealth != null)
        {
            _entityHealth.OnHealthChanged += OnHealthChanged;
        }
    }

    void OnDisable()
    {
        if (_entityHealth != null)
        {
            _entityHealth.OnHealthChanged -= OnHealthChanged;
        }
    }

    public void OnHealthChanged(float currentHealth, float maxHealth)
    {
        _currentHealth = currentHealth;
        _maxHealth = maxHealth;

        _hpBarFill.fillAmount = currentHealth / maxHealth;
        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        if (_healthText != null)
        {
            _healthText.text = $"Health: {_currentHealth:F0}/{_maxHealth:F0}";
        }
    }
}
