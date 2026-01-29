using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerMagnet : MonoBehaviour
{
    [SerializeField] float _magnetRadius = 1.5f;
    [SerializeField] LayerMask _pickupLayer;
    [SerializeField] Light2D _torchLight;

    CircleCollider2D _collider;
    EntityHealth _playerHealth;

    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        if (_collider != null && _torchLight != null)
        {
            _collider.radius = _magnetRadius;
            _torchLight.pointLightOuterRadius = _magnetRadius;
        }

        _playerHealth = GetComponentInParent<EntityHealth>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        bool isFullHealth = true;

        if (_playerHealth.GetCurrentHealth() < _playerHealth.GetMaxHealth())
        {
            isFullHealth = false;
        }

        if (((1 << collision.gameObject.layer) & _pickupLayer) != 0)
        {
            if (collision.TryGetComponent(out Pickup pickup))
            {
                if (pickup.GetPickupType() == Pickup.PickupType.Heal && isFullHealth)
                {
                    return;
                }

                pickup.StartPulling();
            }
        }
    }

    void OnValidate()
    {
        if (_collider == null)
            _collider = GetComponent<CircleCollider2D>();
            
        if (_collider != null)
        {
            _collider.radius = _magnetRadius;
        }
    }

    public void UpgradeTorchRadius(float amount)
    {
        _magnetRadius += amount;
        if (_collider != null)
        {
            _collider.radius = _magnetRadius;
        }
        if (_torchLight != null)
        {
            _torchLight.pointLightOuterRadius = _magnetRadius;
        }
    }

    void OnDrawGizmos()
    {
        // Visualize magnet radius
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, _magnetRadius);
    }

    public float GetTorchRadius() => _magnetRadius;
}
