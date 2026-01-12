using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _travelSpeed;
    [SerializeField] float _damage;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] ParticleSystem _hitParticles;

    public void InitializeProjectile(Vector2 direction)
    {
        Launch(direction);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy projectile upon hitting wall
        if (collision.gameObject.CompareTag("Terrain"))
        {
            DestroyProjectile();
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
