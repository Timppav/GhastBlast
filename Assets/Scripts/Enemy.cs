using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] AudioClip _deathSound;
    [SerializeField] SpriteRenderer _characterBody;
    [SerializeField] Animator _animator;

    [Header("Enemy Specific Config")]
    [SerializeField] float _idleDuration = 2f;
    [SerializeField] float _aggroDistance = 10f;
    [SerializeField] float _initialSpeed = 0.5f;
    [SerializeField] float _aggroSpeed = 3f;

    EntityHealth _entityHealth;
    NavMeshAgent _agent;
    GameObject _target;
    Vector3 _lastPosition;
    float _idleTimer;
    bool _isIdle = true;
    bool _isAggro = false;

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

        if (!_isIdle && _target != null && _agent.enabled)
        {
            _agent.SetDestination(_target.transform.position);
            CheckAggro();
            HandleWalking();
            HandleSpriteFlip();
        }
    }

    void OnDisable()
    {
        _entityHealth.OnDeath -= DestroyEnemy;
    }

    void CheckAggro()
    {
        if (_target != null && Vector3.Distance(_lastPosition, _target.transform.position) <= _aggroDistance)
        {
            _isAggro = true;
        }
    }

    void HandleWalking()
    {
        bool isMoving = _agent.velocity.magnitude > 0.1f;
        _animator.SetBool("isWalking", isMoving);

        if (_isAggro)
        {
            _agent.speed = _aggroSpeed;
        } else {
            _agent.speed = _initialSpeed;
        }

        float animationSpeed = _agent.speed / 3f;
        _animator.SetFloat("speed", animationSpeed);
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
