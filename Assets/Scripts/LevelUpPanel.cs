using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpPanel : MonoBehaviour
{
    [SerializeField] LevelUpManager _levelUpManager;

    [Header("Stat Frames")]
    [SerializeField] Transform _damageFrame;
    [SerializeField] Transform _fireRateFrame;
    [SerializeField] Transform _healthFrame;
    [SerializeField] Transform _healthRegenFrame;
    [SerializeField] Transform _movementSpeedFrame;
    [SerializeField] Transform _torchRangeFrame;

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

    public void DisableAllFrames()
    {
        SetFrameUninteractable(_damageFrame, true);
        SetFrameUninteractable(_fireRateFrame, true);
        SetFrameUninteractable(_healthFrame, true);
        SetFrameUninteractable(_healthRegenFrame, true);
        SetFrameUninteractable(_movementSpeedFrame, true);
        SetFrameUninteractable(_torchRangeFrame, true);
    }

    public void EnableAllFrames()
    {
        SetFrameUninteractable(_damageFrame, false);
        SetFrameUninteractable(_fireRateFrame, false);
        SetFrameUninteractable(_healthFrame, false);
        SetFrameUninteractable(_healthRegenFrame, false);
        SetFrameUninteractable(_movementSpeedFrame, false);
        SetFrameUninteractable(_torchRangeFrame, false);
    }

    public void SetFrameUninteractable(Transform frame, bool isGrayscale)
    {
        if (frame == null) return;

        Image[] images = frame.GetComponentsInChildren<Image>(true);

        foreach (Image image in images)
        {
            if (isGrayscale)
            {
                image.color = new Color(0.5f, 0.5f, 0.5f, image.color.a);
            }
            else
            {
                image.color = new Color(1f, 1f, 1f, image.color.a);
            }
        }

        Button button = frame.GetComponentInChildren<Button>();
        if (button != null)
        {
            button.interactable = !isGrayscale;
        }
    }
}
