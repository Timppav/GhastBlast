using UnityEngine;
using TMPro;

public class PlayerStatValues : MonoBehaviour
{
    Player _player;
    PlayerController _playerController;
    PlayerStaffController _playerStaffController;
    EntityHealth _playerHealth;
    PlayerMagnet _playerMagnet;

    [Header("Stat Value Texts")]
    [SerializeField] TextMeshProUGUI _damageValue;
    [SerializeField] TextMeshProUGUI _fireRateValue;
    [SerializeField] TextMeshProUGUI _healthValue;
    [SerializeField] TextMeshProUGUI _healthRegenValue;
    [SerializeField] TextMeshProUGUI _speedValue;
    [SerializeField] TextMeshProUGUI _torchValue;

    void Start()
    {
        _player = FindFirstObjectByType<Player>();

        if (_player != null)
        {
            _playerController = _player.GetComponent<PlayerController>();
            _playerStaffController = _player.GetComponentInChildren<PlayerStaffController>(true);
            _playerHealth = _player.GetComponent<EntityHealth>();
            _playerMagnet = _player.GetComponentInChildren<PlayerMagnet>(true);

            UpdateDamageValue();
            UpdateFireRateValue();
            UpdateHealthValue();
            UpdateHealthRegenValue();
            UpdateSpeedValue();
            UpdateTorchValue();
        }
    }

    public void UpdateDamageValue()
    {
        if (_damageValue != null && _playerStaffController != null)
        {
            _damageValue.text = _playerStaffController.GetProjectileDamage().ToString("F2");
        }
    }

    public void UpdateFireRateValue()
    {
        if (_fireRateValue != null && _playerStaffController != null)
        {
            _fireRateValue.text = _playerStaffController.GetFireRate().ToString("F2");
        }
    }

    public void UpdateHealthValue()
    {
        if (_healthValue != null && _playerHealth != null)
        {
            _healthValue.text = _playerHealth.GetMaxHealth().ToString("F2");
        }
    }

    public void UpdateHealthRegenValue()
    {
        if (_healthRegenValue != null && _playerHealth != null)
        {
            _healthRegenValue.text = _playerHealth.GetHealthRegen().ToString("F2");
        }
    }

    public void UpdateSpeedValue()
    {
        if (_speedValue != null && _playerController != null)
        {
            _speedValue.text = _playerController.GetMovementSpeed().ToString("F2");
        }
    }

    public void UpdateTorchValue()
    {
        if (_torchValue != null && _playerMagnet != null)
        {
            _torchValue.text = _playerMagnet.GetTorchRadius().ToString("F2");
        }
    }
}
