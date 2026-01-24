using UnityEngine;
using System;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] float _maxHealth;
    [SerializeField] float _currentHealth;
    [SerializeField] float _healthRegen;
    [SerializeField] float _invulnerabilityDuration = 1f;
    [SerializeField] float _healthRegenDelay = 5f;

    float _lastDamageTime;
    float _lastInvulnerabilityTime;
    bool _isInvulnerable;

    public Action OnDeath;
    public Action<float, float> OnHealthChanged;
    public Action OnDamageTaken;

    void Awake()
    {
        _currentHealth = _maxHealth;
        _lastDamageTime = -_healthRegenDelay;
    }

    void Start()
    {
        InvokeRepeating(nameof(HandleHealthRegen), 5f, 5f);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void LoseHealth(float healthLost)
    {
        if (_isInvulnerable)
        {
            return;
        }

        _currentHealth -= healthLost;
        _lastDamageTime = Time.time;
        OnHealthChanged?.Invoke(Mathf.Clamp(_currentHealth, 0, _maxHealth), _maxHealth);
        OnDamageTaken?.Invoke();

        if (_currentHealth <= 0)
        {
            Death();
        } else {
            StartInvulnerability();
        }
    }

    void StartInvulnerability()
    {
        _isInvulnerable = true;
        _lastInvulnerabilityTime = Time.time;
        Invoke(nameof(EndInvulnerability), _invulnerabilityDuration);
    }

    void EndInvulnerability()
    {
        _isInvulnerable = false;
    }

    void HandleHealthRegen()
    {
        float timeSinceLastDamage = Time.time - _lastDamageTime;

        if (_currentHealth < _maxHealth && timeSinceLastDamage >= _healthRegenDelay)
        {
            _currentHealth = Mathf.Clamp(_currentHealth + _healthRegen, 0, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
        
    }

    public void Death()
    {
        OnDeath?.Invoke();
    }

    public float GetCurrentHealth() => _currentHealth;
    public float GetMaxHealth() => _maxHealth;
}
