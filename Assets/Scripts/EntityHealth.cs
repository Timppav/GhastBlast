using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class EntityHealth : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float _maxHealth;
    [SerializeField] float _currentHealth;
    [SerializeField] float _healthRegen;
    [SerializeField] float _healthRegenDelay = 5f;
    [SerializeField] float _invulnerabilityDuration = 1f;
    
    [Header("Visual Feedback")]
    [SerializeField] bool _isPlayer;
    [SerializeField] Light2D _torch;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] float _flashSpeed = 0.1f;

    [SerializeField] AudioClip _hitSound;

    float _lastDamageTime;
    float _lastInvulnerabilityTime;
    bool _isInvulnerable;
    Color _originalTorchColor;
    Color _originalSpriteColor;
    Color _redColor = Color.red;

    public Action OnDeath;
    public Action<float, float> OnHealthChanged;
    public Action OnDamageTaken;

    void Awake()
    {
        _currentHealth = _maxHealth;
        _lastDamageTime = -_healthRegenDelay;

        if (_torch != null)
        {
            _originalTorchColor = _torch.color;
        }
        
        if (_spriteRenderer != null)
        {
            _originalSpriteColor = _spriteRenderer.color;
        }
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

        if  (_hitSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudio (_hitSound, AudioManager.SoundType.SFX, 1.0f, false);
        }

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

        if (_isPlayer)
        {
            // Player: flash torch red and blink sprite
            StartCoroutine(FlashTorchRed());
            StartCoroutine(BlinkSprite());
        }
        else
        {
            // Enemy: flash sprite color red
            StartCoroutine(FlashSpriteRed());
        }

        Invoke(nameof(EndInvulnerability), _invulnerabilityDuration);
    }

    void EndInvulnerability()
    {
        _isInvulnerable = false;

        if (_torch != null)
        {
            _torch.color = _originalTorchColor;
        }

        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = true;
            _spriteRenderer.color = _originalSpriteColor;
        }
    }

    IEnumerator FlashTorchRed() {
        if (_torch == null) yield break;
        
        float fadeDuration = 0.15f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            _torch.color = Color.Lerp(_originalTorchColor, _redColor, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _torch.color = _redColor;
        
        yield return new WaitForSeconds(0.1f);
        
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            _torch.color = Color.Lerp(_redColor, _originalTorchColor, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        _torch.color = _originalTorchColor;
    }

    IEnumerator BlinkSprite()
    {
        if (_spriteRenderer == null) yield break;
        
        float elapsed = 0f;
        
        while (elapsed < _invulnerabilityDuration)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(_flashSpeed);
            
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(_flashSpeed);
            
            elapsed += _flashSpeed * 2;
        }
        
        _spriteRenderer.enabled = true;
    }

    IEnumerator FlashSpriteRed()
    {
        if (_spriteRenderer == null) yield break;
        
        float elapsed = 0f;
        
        while (elapsed < _invulnerabilityDuration)
        {
            _spriteRenderer.color = _redColor;
            yield return new WaitForSeconds(_flashSpeed);
            
            _spriteRenderer.color = _originalSpriteColor;
            yield return new WaitForSeconds(_flashSpeed);
            
            elapsed += _flashSpeed * 2;
        }
        
        _spriteRenderer.color = _originalSpriteColor;
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
