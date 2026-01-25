using UnityEngine;
using System.Collections.Generic;

public class StaffStrike : MonoBehaviour
{
    float _damage;
    HashSet<GameObject> _hitEnemies = new HashSet<GameObject>();

    public void InitializeStrike(float damage, float angle, bool flip)
    {
        _damage = damage;

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
        if (target.TryGetComponent(out EntityHealth entityHealth))
        {
            entityHealth.LoseHealth(_damage);
        }
    }
}
