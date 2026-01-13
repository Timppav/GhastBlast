using UnityEngine;
using System;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] float _maxHealth;
    [SerializeField] float _currentHealth;
    [SerializeField] float _healthRegen;

    public Action OnDeath;
    public Action<float, float> OnHealthChanged;

    void Awake()
    {
        _currentHealth = _maxHealth;
    }

    void Start()
    {
        InvokeRepeating(nameof(HandleHealthRegen), 1f, 1f);
    }

    public void LoseHealth(float healthLost)
    {
        _currentHealth -= healthLost;
        OnHealthChanged?.Invoke(Mathf.Clamp(_currentHealth, 0, _maxHealth), _maxHealth);

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    void HandleHealthRegen()
    {
        if (_currentHealth < _maxHealth)
        {
            _currentHealth = Mathf.Clamp(_currentHealth + _maxHealth * _healthRegen, 0, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
        
    }

    public void Death()
    {
        OnDeath?.Invoke();
    }
}
