using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        ExperienceOrb,
        TripleShotBonus,
        Heal,
        Key
    }

    [SerializeField] PickupType _pickupType = PickupType.ExperienceOrb;

    [Header("Experience Orb Settings")]
    [SerializeField] float _experiencePoints = 20f;

    [Header("Triple Shot Bonus Settings")]
    [SerializeField] float _bonusDuration = 10f;

    [Header("Heal Settings")]
    [SerializeField] float _healPercentage = 0.3f;

    [Header("Magnet Settings")]
    [SerializeField] float _moveSpeed = 10f;

    [Header("Visual Effects")]
    [SerializeField] float _rotationSpeed = 360f;
    [SerializeField] GameObject _pickupParticlePrefab;

    [Header("Audio")]
    [SerializeField] AudioClip _pickupSound;
    [SerializeField] float _volume = 1.0f;

    Transform _player;
    TrailRenderer _trailRenderer;
    bool _isBeingPulled = false;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;

        _trailRenderer = GetComponent<TrailRenderer>();
        if (_trailRenderer != null)
        {
            _trailRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (_isBeingPulled && _player != null)
        {
            // Accelerate as it gets closer
            transform.position = Vector3.MoveTowards(
                transform.position, 
                _player.position, 
                _moveSpeed * Time.deltaTime
            );

            transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }
    }

    public void StartPulling()
    {
        if (_isBeingPulled) return;
        
        _isBeingPulled = true;
        
        if (_trailRenderer != null)
        {
            _trailRenderer.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (_pickupType == PickupType.Heal)
        {
            if (collision.gameObject.TryGetComponent(out EntityHealth playerHealth))
            {
                if (playerHealth.GetCurrentHealth() >= playerHealth.GetMaxHealth())
                {
                    return;
                }
            }
        }

        if (_pickupParticlePrefab != null)
        {
            GameObject particles = Instantiate(_pickupParticlePrefab, transform.position, Quaternion.identity);
        }

        if (collision.gameObject.TryGetComponent(out Player player))
        {
            switch (_pickupType)
            {
                case PickupType.ExperienceOrb:
                    player.AddExperience(_experiencePoints);
                    break;
                case PickupType.TripleShotBonus:
                    player.GetComponentInChildren<PlayerStaffController>().ActivateTripleShotBonus(_bonusDuration);
                    break;
                case PickupType.Heal:
                    player.GetComponentInParent<EntityHealth>().HealPercentageOfMaxHealth(_healPercentage);
                    break;
                case PickupType.Key:
                    player.AddKey();
                    break;
            }

            if (_pickupSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayAudio(_pickupSound, AudioManager.SoundType.SFX, _volume, false);
            }

            Destroy(gameObject);
        }
    }

    public PickupType GetPickupType()
    {
        return _pickupType;
    }
}
