using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    [SerializeField] float _magnetRadius = 5f;
    [SerializeField] LayerMask _expOrbLayer;

    CircleCollider2D _collider;

    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        if (_collider != null)
        {
            _collider.radius = _magnetRadius;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _expOrbLayer) != 0)
        {
            if (collision.TryGetComponent(out EXPOrb orb))
            {
                orb.StartPulling();
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

    void OnDrawGizmos()
    {
        // Visualize magnet radius
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, _magnetRadius);
    }
}
