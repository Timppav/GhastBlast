using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    Player _player;
    PlayerController _playerController;
    PlayerStaffController _playerStaffController;
    EntityHealth _playerHealth;
    PlayerMagnet _playerMagnet;
    InGameUIManager _inGameUIManager;
    PlayerStatValues _playerStatValues;

    [Header("Stat Upgrade Values")]
    [SerializeField] float _damageUpgrade = 0.5f;
    [SerializeField] float _fireRateUpgrade = 0.3f;
    [SerializeField] float _healthUpgrade = 10f;
    [SerializeField] float _healthRegenUpgrade = 1f;
    [SerializeField] float _movementSpeedUpgrade = 0.3f;
    [SerializeField] float _torchRangeUpgrade = 0.5f;

    void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _inGameUIManager = FindFirstObjectByType<InGameUIManager>();
        _playerStatValues = FindFirstObjectByType<PlayerStatValues>();

        if (_player != null)
        {
            _playerController = _player.GetComponent<PlayerController>();
            _playerStaffController = _player.GetComponentInChildren<PlayerStaffController>();
            _playerHealth = _player.GetComponent<EntityHealth>();
            _playerMagnet = _player.GetComponentInChildren<PlayerMagnet>();

            _player.OnLevelUp += HandleLevelUp;
        }
    }

    void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnLevelUp -= HandleLevelUp;
        }
    }

    void HandleLevelUp(float newLevel)
    {
        _inGameUIManager?.ShowLevelUpPanel();
    }

    public void UpgradeDamage()
    {
        if (_playerStaffController != null)
        {
            _playerStaffController.UpgradeDamage(_damageUpgrade);
            _playerStatValues.UpdateDamageValue();
        }
        CloseLevelUpPanel();
    }

    public void UpgradeFireRate()
    {
        if (_playerStaffController != null)
        {
            _playerStaffController.UpgradeFireRate(_fireRateUpgrade);
            _playerStatValues.UpdateFireRateValue();
        }
        CloseLevelUpPanel();
    }

    public void UpgradeHealth()
    {
        if (_playerHealth != null)
        {
            _playerHealth.UpgradeMaxHealth(_healthUpgrade);
            _playerStatValues.UpdateHealthValue();
        }
        CloseLevelUpPanel();
    }

    public void UpgradeHealthRegen()
    {
        if (_playerHealth != null)
        {
            _playerHealth.UpgradeHealthRegen(_healthRegenUpgrade);
            _playerStatValues.UpdateHealthRegenValue();
        }
        CloseLevelUpPanel();
    }

    public void UpgradeMovementSpeed()
    {
        if (_playerController != null)
        {
            _playerController.UpgradeMovementSpeed(_movementSpeedUpgrade);
            _playerStatValues.UpdateSpeedValue();
        }
        CloseLevelUpPanel();
    }

    public void UpgradeTorchRange()
    {
        if (_playerMagnet != null)
        {
            _playerMagnet.UpgradeTorchRadius(_torchRangeUpgrade);
            _playerStatValues.UpdateTorchValue();
        }
        CloseLevelUpPanel();
    }

    void CloseLevelUpPanel()
    {
        _inGameUIManager?.HideLevelUpPanel();
    }
}
