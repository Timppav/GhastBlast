using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _travelSpeed;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] ParticleSystem _hitParticles;

    float _damage;

    public void InitializeProjectile(Vector2 direction, float damage)
    {
        _damage = damage;
        Launch(direction);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DealDamage(collision.gameObject);
            DestroyProjectile();
        }

        if (collision.gameObject.CompareTag("Terrain"))
        {
            DestroyProjectile();
        }
    }

    void DealDamage(GameObject target)
    {
        if (target.TryGetComponent(out EntityHealth entityHealth))
        {
            entityHealth.LoseHealth(_damage);
        }
    }

    void Launch(Vector2 direction)
    {
        Vector2 movement = direction.normalized * _travelSpeed;
        _rb.linearVelocity = movement;
    }

    void DestroyProjectile()
    {
        ParticleSystem hitParticles = Instantiate(_hitParticles, transform.position, Quaternion.identity);
        Destroy(hitParticles.gameObject, 1f);
        Destroy(gameObject);
    }
}
