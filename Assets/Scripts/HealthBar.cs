using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] bool _isPlayer = false;
    EntityHealth _entityHealth;
    Image _hpBarFill;
    TextMeshProUGUI _healthText;

    float _currentHealth;
    float _maxHealth;

    void Awake()
    {
        if (_isPlayer)
        {
            Transform fillTransform = transform.Find("Fill");
            if (fillTransform != null)
            {
                _hpBarFill = fillTransform.GetComponent<Image>();
            }
        }
        else
        {
            Transform healthBarTransform = transform.Find("HealthBar");
            if (healthBarTransform != null)
            {
                Transform fillTransform = healthBarTransform.Find("Fill");
                if (fillTransform != null)
                {
                    _hpBarFill = fillTransform.GetComponent<Image>();
                }
            }

            gameObject.SetActive(false);
        }
        
        _healthText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        if (!_isPlayer && _entityHealth == null)
        {
            _entityHealth = GetComponentInParent<EntityHealth>();
        }

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
            _entityHealth.OnHealthChanged += OnHealthChanged;
            _currentHealth = _entityHealth.GetCurrentHealth();
            _maxHealth = _entityHealth.GetMaxHealth();

            StartCoroutine(UpdateHealthBarNextFrame());
        }
    }

    void OnDisable()
    {
        if (_entityHealth != null)
        {
            _entityHealth.OnHealthChanged -= OnHealthChanged;
        }
    }

    void OnDestroy()
    {
        if (_entityHealth != null)
        {
            _entityHealth.OnHealthChanged -= OnHealthChanged;
        }
    }

    IEnumerator UpdateHealthBarNextFrame()
    {
        yield return null; // Wait one frame
        UpdateHealthBar();
    }

    public void OnHealthChanged(float currentHealth, float maxHealth)
    {
        _currentHealth = currentHealth;
        _maxHealth = maxHealth;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (_hpBarFill != null)
        {
            _hpBarFill.fillAmount = _currentHealth / _maxHealth;
        }

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
