using UnityEngine;
using System.Collections.Generic;

public class StaffStrike : MonoBehaviour
{
    [SerializeField] float _knockbackForce = 5f;

    float _damage;
    Vector2 _knockbackDirection;
    HashSet<GameObject> _hitEnemies = new HashSet<GameObject>();

    public void InitializeStrike(float damage, float angle, bool flip, Vector2 knockbackDirection)
    {
        _damage = damage;
        _knockbackDirection = knockbackDirection;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3 scale = transform.localScale;
        scale.y = flip ? -1f : 1f;
        transform.localScale = scale;

        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                float animationLength = clipInfo[0].clip.length;
                Destroy(gameObject, animationLength);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !_hitEnemies.Contains(collision.gameObject))
        {
            DealDamage(collision.gameObject);
            _hitEnemies.Add(collision.gameObject);
        }
    }

    void DealDamage(GameObject target)
    {
        EntityHealth entityHealth = target.GetComponent<EntityHealth>();
        if (entityHealth == null) return;

        float currentHealth = entityHealth.GetCurrentHealth();

        entityHealth.LoseHealth(_damage);

        if (currentHealth > _damage)
        {
            if (target.TryGetComponent(out Rigidbody2D rb))
            {
                // Stop current velocity for instant knockback
                rb.linearVelocity = Vector2.zero;
                
                // Apply knockback in the direction of the player's cursor
                rb.AddForce(_knockbackDirection * _knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
