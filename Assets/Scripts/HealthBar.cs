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
