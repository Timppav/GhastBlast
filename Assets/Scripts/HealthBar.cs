using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image _hpBarFill;
    [SerializeField] EntityHealth _entityHealth;

    void OnEnable()
    {
        if (_entityHealth != null)
        {
            _entityHealth.OnHealthChanged += OnHealthChanged;
        }
    }

    public void SetEntityHealth(EntityHealth entityHealth)
    {
        // Unsubscribe from old entity if exists
        if (_entityHealth != null)
        {
            _entityHealth.OnHealthChanged -= OnHealthChanged;
        }

        // Set new entity and subscribe
        _entityHealth = entityHealth;
        if (_entityHealth != null)
        {
            _entityHealth.OnHealthChanged += OnHealthChanged;
            // Initialize the health bar with current health
            OnHealthChanged(_entityHealth.GetCurrentHealth(), _entityHealth.GetMaxHealth());
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
        _hpBarFill.fillAmount = currentHealth / maxHealth;
    }
}
