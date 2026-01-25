using UnityEngine;
using TMPro;

public class LevelUpPanel : MonoBehaviour
{
    [SerializeField] LevelUpManager _levelUpManager;

    [Header("Stat Texts")]
    [SerializeField] TextMeshProUGUI _damageText;
    [SerializeField] TextMeshProUGUI _fireRateText;
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] TextMeshProUGUI _healthRegenText;
    [SerializeField] TextMeshProUGUI _movementSpeedText;
    [SerializeField] TextMeshProUGUI _torchRangeText;

    void Start()
    {
        if (_levelUpManager != null)
        {
            if (_damageText != null)
            {
                _damageText.text = string.Format("Damage +{0}", _levelUpManager.GetDamageUpgrade().ToString());
            }

            if (_fireRateText != null)
            {
                _fireRateText.text = string.Format("+{0} Fire Rate", _levelUpManager.GetFireRateUpgrade().ToString());
            }

            if (_healthText != null)
            {
                _healthText.text = string.Format("Health +{0}", _levelUpManager.GetHealthUpgrade().ToString());
            }

            if (_healthRegenText != null)
            {
                _healthRegenText.text = string.Format("+{0} Health Regen", _levelUpManager.GetHealthRegenUpgrade().ToString());
            }

            if (_movementSpeedText != null)
            {
                _movementSpeedText.text = string.Format("Movement Speed +{0}", _levelUpManager.GetMovementSpeedUpgrade().ToString());
            }

            if (_torchRangeText != null)
            {
                _torchRangeText.text = string.Format("+{0} Torch & Pickup Range", _levelUpManager.GetTorchRangeUpgrade().ToString());
            }
        }
    }
}
