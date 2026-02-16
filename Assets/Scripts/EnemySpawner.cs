using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy[] _enemyPrefabs;
    [SerializeField] float _spawnCooldown;
    [SerializeField] float _spawnCooldownReductionMultiplier;
    [SerializeField] float _initialSpawnDelay = 15f;
    float _currentCooldown;
    float _spawnTimer = 0f;

    [SerializeField] Tilemap _groundTiles;
    List<Vector3> _spawnPositions = new();

    [Header("Spawn Safety")]
    [SerializeField] float _minimumSpawnDistance = 5f;

    [Header("Spawn Warning")]
    [SerializeField] GameObject _spawnWarningPrefab;
    [SerializeField] float _warningDuration = 1f;

    [Header("Enemy Pool Settings")]
    [SerializeField] int _maxEnemiesAlive = 9999;
    
    [Header("Spawn Validation")]
    [SerializeField] LayerMask _obstacleLayer;

    Transform _player;

    List<Enemy> _activeEnemies = new();
    Dictionary<Enemy, Queue<Enemy>> _enemyPools = new();

    void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) _player = playerObj.transform;

        SetEnemySpawnPositions();
        InitializeEnemyPools();
        InvokeRepeating(nameof(HandleGameDifficultyIncrease), 5f, 5f);
    }

    void Update()
    {
        // Wait for initial delay before starting to spawn
        if (_spawnTimer < _initialSpawnDelay)
        {
            _spawnTimer += Time.deltaTime;
            return;
        }

        HandleEnemySpawning();
    }

    void InitializeEnemyPools()
    {
        foreach (Enemy enemyPrefab in _enemyPrefabs)
        {
            Queue<Enemy> pool = new Queue<Enemy>();

            for (int i = 0; i < 15; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab);
                float aggroSpeed = enemy.GetAggroSpeed();
                enemy.SetInitialSpeed(aggroSpeed * Random.Range(0.1f, 0.5f));
                enemy.gameObject.SetActive(false);
                enemy.transform.SetParent(transform);
                pool.Enqueue(enemy);
            }

            _enemyPools[enemyPrefab] = pool;
        }
    }

    Enemy GetPooledEnemy(Enemy prefab)
    {
        if (_enemyPools[prefab].Count > 0)
        {
            Enemy enemy = _enemyPools[prefab].Dequeue();
            return enemy;
        } else {
            Enemy enemy = Instantiate(prefab);
            enemy.transform.SetParent(transform);
            return enemy;
        }
    }

    public void ReturnEnemyToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        _activeEnemies.Remove(enemy);

        foreach (var kvp in _enemyPools)
        {
            if (enemy.name.Contains(kvp.Key.name))
            {
                kvp.Value.Enqueue(enemy);
                break;
            }
        }
    }

    void HandleEnemySpawning()
    {
        _currentCooldown -= Time.deltaTime;
        if (_currentCooldown > Time.time) return;

        if (_activeEnemies.Count >= _maxEnemiesAlive)
        {
            Debug.Log("Max enemies reached: " + _maxEnemiesAlive);
            return;
        }

        _currentCooldown = Time.time + _spawnCooldown;
        StartCoroutine(SpawnEnemyToRandomLocation());
    }

    void SetEnemySpawnPositions()
    {
        foreach (Vector3Int position in _groundTiles.cellBounds.allPositionsWithin)
        {
            if (_groundTiles.HasTile(position))
            {
                Vector3 worldPos = _groundTiles.GetCellCenterWorld(position);

                // Check if this position overlaps with obstacles
                if (!Physics2D.OverlapCircle(worldPos, 0.3f, _obstacleLayer))
                {
                    _spawnPositions.Add(worldPos);
                }
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 position;
        int maxAttempts = 50;
        int attempts = 0;

        do
        {
            position = _spawnPositions[Random.Range(0, _spawnPositions.Count)];
            attempts++;

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Could not find safe spawn position after " + maxAttempts);
                break;
            }
        } while (_player != null && Vector3.Distance(position, _player.position) < _minimumSpawnDistance);

        return position;
    }

    Enemy GetRandomEnemyPrefab()
    {
        return _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
    }

    IEnumerator SpawnEnemyToRandomLocation()
    {
        Vector3 spawnPosition = GetRandomPosition();

        // Show warning indicator
        GameObject warning = null;
        if (_spawnWarningPrefab != null)
        {
            warning = Instantiate(_spawnWarningPrefab, spawnPosition, Quaternion.identity);
        }

        // Wait for warning duration
        yield return new WaitForSeconds(_warningDuration);

        if (warning != null)
        {
            Destroy(warning);
        }

        Enemy enemyPrefab = GetRandomEnemyPrefab();
        Enemy enemy = GetPooledEnemy(enemyPrefab);
        enemy.transform.position = spawnPosition;
        enemy.gameObject.SetActive(true);
        _activeEnemies.Add(enemy);
    }

    void HandleGameDifficultyIncrease()
    {
        _spawnCooldown *= _spawnCooldownReductionMultiplier;
    }
}