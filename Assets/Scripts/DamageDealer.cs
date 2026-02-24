using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] float _damage = 1f;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (collision.gameObject.TryGetComponent(out EntityHealth entityHealth))
        {
            entityHealth.LoseHealth(_damage);
        }
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public float GetDamage() => _damage;
}
