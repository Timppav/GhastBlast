using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] float _playerExp;
    [SerializeField] float _nextLevelExp = 100f;
    [SerializeField] float _playerLevel = 1f;
    [SerializeField] float _maxPlayerLevel = 9999f;
    [SerializeField] float _levelExpIncrement = 100f;
    [SerializeField] AudioClip _levelUpSound;
    [SerializeField] AudioClip _deathSound;

    EntityHealth _playerHealth;
    PlayerController _playerController;
    PlayerStaffController _playerStaffController;
    CircleCollider2D _collider;
    Rigidbody2D _rb;

    public Action<float, float> OnExpChanged;
    public Action<float> OnLevelUp;

    void OnEnable()
    {
        _playerHealth = GetComponent<EntityHealth>();
        _playerController = GetComponent<PlayerController>();
        _playerStaffController = GetComponentInChildren<PlayerStaffController>();
        _collider = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _playerHealth.OnDeath += HandleDeath;
    }

    void Start()
    {
        OnExpChanged?.Invoke(_playerExp, _nextLevelExp);
    }

    void OnDisable()
    {
        _playerHealth.OnDeath -= HandleDeath;
    }

    public void AddExperience(float amount) {
        if (_playerLevel >= _maxPlayerLevel)
        {
            return;
        }

        _playerExp += amount;
        OnExpChanged?.Invoke(_playerExp, _nextLevelExp);

        while (_playerExp >= _nextLevelExp && _playerLevel < _maxPlayerLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        _playerLevel++;
        _playerExp -= _nextLevelExp; // Carry over excess exp
        _nextLevelExp += _levelExpIncrement;

        if (_levelUpSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudio(_levelUpSound, AudioManager.SoundType.SFX, 1.0f, false);
        }

        OnLevelUp?.Invoke(_playerLevel);
        OnExpChanged?.Invoke(_playerExp, _nextLevelExp);
    }

    public float GetPlayerLevel() => _playerLevel;
    public float GetPlayerExp() => _playerExp;
    public float GetNextLevelExp() => _nextLevelExp;
    
    public void HandleDeath()
    {
        _rb.linearDamping = 10f;
        _rb.angularDamping = 10f;

        _playerController._animator.SetBool("isDead", true);
        _collider.enabled = false;
        _playerController.enabled = false;

        if (_playerStaffController != null)
        {
            _playerStaffController.enabled = false;
        }

        if (_deathSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.StopCurrentMusic();
            AudioManager.Instance.PlayAudio(_deathSound, AudioManager.SoundType.SFX, 3f, false);
        }

        StartCoroutine(GameOverAfterDelay(2f));
    }

    IEnumerator GameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.GameOver();
    }
}
