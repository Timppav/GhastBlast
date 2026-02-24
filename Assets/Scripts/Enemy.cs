using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] AudioClip _deathSound;
    [SerializeField] SpriteRenderer _characterBody;
    [SerializeField] Animator _animator;
    [SerializeField] CircleCollider2D _damageDealer;

    [Header("Enemy Specific Config")]
    [SerializeField] float _idleDuration = 2f;
    [SerializeField] float _aggroDistance = 10f;
    [SerializeField] float _initialSpeed = 0.5f;
    [SerializeField] float _aggroSpeed = 3f;

    [Header("Drops")]
    [SerializeField] GameObject _expOrbPrefab;

    CircleCollider2D _collider;
    EntityHealth _entityHealth;
    NavMeshAgent _agent;
    Transform _canvas;
    GameObject _target;
    EnemySpawner _spawner;
    float _idleTimer;
    bool _isIdle = true;
    bool _isAggro = false;
    bool _isDead = false;

    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _entityHealth = GetComponent<EntityHealth>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    void OnEnable()
    {
        if (_entityHealth != null)
        {
            _entityHealth.ResetHealth();
            _entityHealth.OnDeath += DestroyEnemy;
            _entityHealth.OnDamageTaken += HandleDamageTaken;
        }
    }

    void Start()
    {
        _idleTimer = _idleDuration;
        _canvas = transform.Find("Canvas");
        _target = GameObject.FindGameObjectWithTag("Player");

        _spawner = FindFirstObjectByType<EnemySpawner>();

        // Check if this enemy was spawned by the spawner or manually placed
        if (_spawner != null && transform.parent != _spawner.transform)
        {
            _spawner = null; // This is a manually placed enemy
        }
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
        _entityHealth.OnDamageTaken -= HandleDamageTaken;
    }

    void HandleDamageTaken()
    {
        _isAggro = true;

        if (_canvas != null)
        {
            _canvas.gameObject.SetActive(true);
        }
    }

    void CheckAggro()
    {
        if (_isAggro) return;
        
        if (_target != null && Vector3.Distance(transform.position, _target.transform.position) <= _aggroDistance)
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
        if (_isDead) return;

        _isDead = true;

        _collider.enabled = false;
        _damageDealer.enabled = false;
        _animator.SetBool("isDead", true);
        _agent.enabled = false;
        AudioManager.Instance.PlayAudio(_deathSound, AudioManager.SoundType.SFX, 1.0f, false);
        DropExpOrb();
        StartCoroutine(ReturnToPoolAfterDelay(0.6f));
    }

    void DropExpOrb()
    {
        if (_expOrbPrefab != null)
        {
            Instantiate(_expOrbPrefab, transform.position, Quaternion.identity);
        }
    }

    IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Reset enemy state
        _animator.SetBool("isDead", false);
        _animator.SetBool("isWalking", false);
        _collider.enabled = true;
        _damageDealer.enabled = true;
        _agent.enabled = true;
        _isIdle = true;
        _isAggro = false;
        _idleTimer = _idleDuration;
        _isDead = false;

        if (_canvas != null)
        {
            _canvas.gameObject.SetActive(false);
        }
        
        if (_spawner != null)
        {
            _spawner.ReturnEnemyToPool(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetInitialSpeed(float speed)
    {
        _initialSpeed = speed;
    }

    public void SetAggroSpeed(float speed)
    {
        _aggroSpeed = speed;
    }

    public float GetAggroSpeed() => _aggroSpeed;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _aggroDistance);
    }
}
