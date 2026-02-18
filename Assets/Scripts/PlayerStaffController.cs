using UnityEngine;
using System.Collections;

public class PlayerStaffController : MonoBehaviour
{
    [SerializeField] Projectile _projectile;
    [SerializeField] StaffStrike _strike;
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioClip _strikeSound;
    [SerializeField] Transform _tip;
    [SerializeField] Transform _strikeDistance;
    [SerializeField] float _projectileDamage = 6f;
    [SerializeField] float _strikeDamage = 18f;
    [SerializeField] float _shootFireRate;
    [SerializeField] Transform _flashlight;
    [SerializeField] float _flashlightDefaultZRotation = -90f;

    float _strikeFireRate;
    float _nextShootFireTime;
    float _nextStrikeFireTime;
    bool _tripleShotBonusActive = false;
    Coroutine _tripleShotCoroutine;
    float _tripleShotRemainingTime = 0f;
    Vector2 _lookDirection;

    void Update()
    {
        if (Time.timeScale == 0f) return; // Disable staff rotation & shooting if game is paused
        
        SetLookDirection();
        RotateStaff();
        HandleSpriteFlip();

        _strikeFireRate = _shootFireRate * 0.3f;

        // Shoots when fire button is pressed/held down
        if (Input.GetButton("Fire1") && Time.time >= _nextShootFireTime)
        {
            _nextShootFireTime = Time.time + 1f / _shootFireRate; // Calculates the next time player can shoot again
            Shoot();
        }

        if (Input.GetButton("Fire2") && Time.time >= _nextStrikeFireTime)
        {
            _nextStrikeFireTime = Time.time + 1f / _strikeFireRate; // Calculates the next time player can use special again
            Strike();
        }
    }

    void RotateStaff()
    {
        float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Shoot()
    {
        if (_tripleShotBonusActive)
        {
            // Shoot 3 projectiles in a fan pattern
            for (int i = -1; i <= 1; i++)
            {
                float angleOffset = i * 15f; // Calculate projectile angle offset
                Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * _lookDirection;

                Projectile newProjectile = Instantiate(_projectile, _tip.position, Quaternion.identity);
                newProjectile.InitializeProjectile(spreadDirection, _projectileDamage);
            }
        } 
        else
        {
            Projectile newProjectile = Instantiate(_projectile, _tip.position, Quaternion.identity);
            newProjectile.InitializeProjectile(_lookDirection, _projectileDamage);
        }

        AudioManager.Instance.PlayAudio(_shootSound, AudioManager.SoundType.SFX, 0.4f, false);
    }

    void Strike()
    {
        AudioManager.Instance.PlayAudio(_strikeSound, AudioManager.SoundType.SFX, 0.4f, false);
        StaffStrike strikeInstance = Instantiate(_strike, _strikeDistance.position, Quaternion.identity);

        float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool flipStrike = mousePosition.x < transform.position.x;
        
        strikeInstance.InitializeStrike(_strikeDamage, angle, flipStrike, _lookDirection);

        strikeInstance.transform.SetParent(transform);
    }

    public void ActivateTripleShotBonus(float duration)
    {
        if (_tripleShotBonusActive)
        {
            _tripleShotRemainingTime += duration;

            if (_tripleShotCoroutine != null)
            {
                StopCoroutine(_tripleShotCoroutine);
            }

            _tripleShotCoroutine = StartCoroutine(TripleShotBonusCoroutine(_tripleShotRemainingTime));
        }
        else
        {
            _tripleShotRemainingTime = duration;
            _tripleShotCoroutine = StartCoroutine(TripleShotBonusCoroutine(duration));
        }
        
        FindFirstObjectByType<InGameUIManager>().ShowTripleShotTimerPanel(duration);
    }

    private IEnumerator TripleShotBonusCoroutine(float duration)
    {
        _tripleShotBonusActive = true;
        _tripleShotRemainingTime = duration;

        while (_tripleShotRemainingTime > 0)
        {
            _tripleShotRemainingTime -= Time.deltaTime;
            yield return null;
        }

        _tripleShotRemainingTime = 0f;
        _tripleShotBonusActive = false;
    }

    void SetLookDirection()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _lookDirection = (mousePosition - (Vector2)transform.position).normalized;
    }

    void HandleSpriteFlip()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool flipStaff = mousePosition.x < transform.position.x;
        
        // Flip the entire staff GameObject
        Vector3 scale = transform.localScale;
        scale.y = flipStaff ? -1f : 1f;
        transform.localScale = scale;
        
        // Counter-flip the flashlight so it stays correctly oriented
        if (_flashlight != null)
        {
            Vector3 flashlightScale = _flashlight.localScale;
            flashlightScale.y = flipStaff ? -1f : 1f;
            _flashlight.localScale = flashlightScale;
            
            // Also invert the Z rotation to maintain correct angle
            Vector3 flashlightRotation = _flashlight.localEulerAngles;
            flashlightRotation.z = flipStaff ? -_flashlightDefaultZRotation : _flashlightDefaultZRotation;
            _flashlight.localEulerAngles = flashlightRotation;
        }
    }

    public void UpgradeDamage(float amount)
    {
        _projectileDamage += amount;
        _strikeDamage += amount * 2f;
    }

    public void UpgradeFireRate(float amount)
    {
        _shootFireRate += amount;
        _strikeFireRate += amount;
    }

    public float GetProjectileDamage() => _projectileDamage;
    public float GetFireRate() => _shootFireRate;
}
