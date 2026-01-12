using UnityEngine;

public class PlayerStaffController : MonoBehaviour
{
    [SerializeField] Projectile _projectile;
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioClip _specialSound;
    [SerializeField] Transform _tip;
    [SerializeField] float _shootFireRate;
    [SerializeField] float _specialFireRate;
    [SerializeField] float _specialSpreadAngle = 15f;
    [SerializeField] Transform _flashlight;
    [SerializeField] float _flashlightDefaultZRotation = -90f;
    float _nextShootFireTime;
    float _nextSpecialFireTime;
    Vector2 _lookDirection;

    void Update()
    {
        if (Time.timeScale == 0f) return; // Disable staff rotation & shooting if game is paused
        
        SetLookDirection();
        RotateStaff();
        HandleSpriteFlip();

        // Shoots when fire button is pressed/held down
        if (Input.GetButton("Fire1") && Time.time >= _nextShootFireTime)
        {
            _nextShootFireTime = Time.time + 1f / _shootFireRate; // Calculates the next time player can shoot again
            Shoot();
        }

        if (Input.GetButton("Fire2") && Time.time >= _nextSpecialFireTime)
        {
            _nextSpecialFireTime = Time.time + 1f / _specialFireRate; // Calculates the next time player can use special again
            Special();
        }
    }

    void RotateStaff()
    {
        float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Shoot()
    {
        AudioManager.Instance.PlayAudio(_shootSound, AudioManager.SoundType.SFX, 0.4f, false);
        Projectile newProjectile = Instantiate(_projectile, _tip.position, Quaternion.identity);
        newProjectile.InitializeProjectile(_lookDirection);
    }

    void Special()
    {
        AudioManager.Instance.PlayAudio(_specialSound, AudioManager.SoundType.SFX, 0.4f, false);
        
        // Shoot 3 projectiles in a fan pattern
        for (int i = -1; i <= 1; i++)
        {
            float angleOffset = i * _specialSpreadAngle; // Calculate projectile angle offset

            Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * _lookDirection;

            Projectile newProjectile = Instantiate(_projectile, _tip.position, Quaternion.identity);
            newProjectile.InitializeProjectile(spreadDirection);
        }
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
}
