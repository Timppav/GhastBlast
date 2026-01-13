using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] AudioClip _deathSound;
    EntityHealth _entityHealth;

    void Awake()
    {
        _entityHealth = GetComponent<EntityHealth>();
    }

    void Start()
    {
        _entityHealth.OnDeath += DestroyEnemy;
    }

    void OnDisable()
    {
        _entityHealth.OnDeath -= DestroyEnemy;
    }

    public void DestroyEnemy()
    {
        AudioManager.Instance.PlayAudio(_deathSound, AudioManager.SoundType.SFX, 1.0f, false);
        Destroy(gameObject);
    }
}
