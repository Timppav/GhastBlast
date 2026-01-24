using UnityEngine;

public class EXPOrb : MonoBehaviour
{
    [SerializeField] float _experiencePoints = 10f;
    [SerializeField] AudioClip _pickupSound;

    [Header("Magnet Settings")]
    [SerializeField] float _moveSpeed = 8f;

    [Header("Visual Effects")]
    [SerializeField] float _rotationSpeed = 360f;
    [SerializeField] GameObject _pickupParticlePrefab;

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
        
        // Enable trail when orb starts moving
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

        if (_pickupParticlePrefab != null)
        {
            GameObject particles = Instantiate(_pickupParticlePrefab, transform.position, Quaternion.identity);
        }

        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.AddExperience(_experiencePoints);

            if (_pickupSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayAudio(_pickupSound, AudioManager.SoundType.SFX, 1.0f, false);
            }

            Destroy(gameObject);
        }
    }
}
