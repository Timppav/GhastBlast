using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] AudioClip _deathSound;
    [SerializeField] SpriteRenderer _characterBody;
    [SerializeField] Animator _animator;
    [SerializeField] float _idleDuration = 2f;

    EntityHealth _entityHealth;
    NavMeshAgent _agent;
    GameObject _target;
    Vector3 _lastPosition;
    float _idleTimer;
    bool _isIdle = true;

    void Awake()
    {
        _entityHealth = GetComponent<EntityHealth>();

        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    void Start()
    {
        _idleTimer = _idleDuration;
        _target = GameObject.FindGameObjectWithTag("Player");
        _lastPosition = transform.position;

        _entityHealth.OnDeath += DestroyEnemy;
    }

    void Update()
    {
        if (_isIdle)
        {
            _idleTimer -= Time.deltaTime;
            if (_idleTimer <= 0f)
            {
                _isIdle = false;
            }
            return;
        }
        
        if (_target != null && _agent.enabled)
        {
            _agent.SetDestination(_target.transform.position);
            HandleAnimation();
            HandleSpriteFlip();
        }
    }

    void OnDisable()
    {
        _entityHealth.OnDeath -= DestroyEnemy;
    }

    void HandleAnimation()
    {
        bool isMoving = _agent.velocity.magnitude > 0.1f;
        _animator.SetBool("isWalking", isMoving);
    }

    void HandleSpriteFlip()
    {
        // Flip sprite based on movement direction
        if (_agent.velocity.x < -0.1f)
        {
            _characterBody.flipX = true;
        }
        else if (_agent.velocity.x > 0.1f)
        {
            _characterBody.flipX = false;
        }
    }

    public void DestroyEnemy()
    {
        _animator.SetBool("isDead", true);
        _agent.enabled = false;
        AudioManager.Instance.PlayAudio(_deathSound, AudioManager.SoundType.SFX, 1.0f, false);
        Destroy(gameObject, 0.6f);
    }
}
