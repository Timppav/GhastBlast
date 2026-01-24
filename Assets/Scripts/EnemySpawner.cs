using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy[] _enemyPrefabs;
    [SerializeField] float _spawnCooldown;
    [SerializeField] float _spawnCooldownReductionMultiplier;
    float _currentCooldown;

    [SerializeField] Tilemap _groundTiles;
    List<Vector3> _spawnPositions = new();

    [Header("Spawn Safety")]
    [SerializeField] Transform _player;
    [SerializeField] float _minimumSpawnDistance = 5f;

    [Header("Spawn Warning")]
    [SerializeField] GameObject _spawnWarningPrefab;
    [SerializeField] float _warningDuration = 1f;

    [Header("Enemy Pool Settings")]
    [SerializeField] int _maxEnemiesAlive = 100;
    List<Enemy> _activeEnemies = new();
    Dictionary<Enemy, Queue<Enemy>> _enemyPools = new();

    void Start()
    {
        SetEnemySpawnPositions();
        InitializeEnemyPools();
        InvokeRepeating(nameof(HandleGameDifficultyIncrease), 1f, 1f);
    }

    void Update()
    {
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
                _spawnPositions.Add(_groundTiles.GetCellCenterWorld(position));
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

        Instantiate(GetRandomEnemyPrefab(), spawnPosition, Quaternion.identity);
    }

    void HandleGameDifficultyIncrease()
    {
        _spawnCooldown *= _spawnCooldownReductionMultiplier;
    }
}