using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] float _playerExp;
    [SerializeField] float _nextLevelExp = 100f;
    [SerializeField] float _playerLevel = 1f;
    [SerializeField] float _maxPlayerLevel = 99f;
    [SerializeField] float _levelExpIncrement = 100f;
    [SerializeField] AudioClip _levelUpSound;

    EntityHealth _playerHealth;
    PlayerController _playerController;
    PlayerStaffController _playerStaffController;

    private void OnEnable()
    {
        _playerHealth = GetComponent<EntityHealth>();
        _playerController = GetComponent<PlayerController>();
        _playerStaffController = GetComponentInChildren<PlayerStaffController>();
        _playerHealth.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _playerHealth.OnDeath -= HandleDeath;
    }

    public void AddExperience(float amount) {
        if (_playerLevel >= _maxPlayerLevel)
        {
            Debug.Log("Max level reached!");
            return;
        }

        _playerExp += amount;
        Debug.Log($"Gained {amount} exp. Current: {_playerExp}/{_nextLevelExp}");

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

        Debug.Log($"Level Up! Now level {_playerLevel}");
    }

    public float GetPlayerLevel() => _playerLevel;
    public float GetPlayerExp() => _playerExp;
    public float GetNextLevelExp() => _nextLevelExp;
    
    public void HandleDeath()
    {
        _playerController._animator.SetBool("isDead", true);
        _playerController.enabled = false;
        if (_playerStaffController != null)
            _playerStaffController.enabled = false;

        StartCoroutine(GameOverAfterDelay(2f));
    }

    IEnumerator GameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.GameOver();
    }
}
