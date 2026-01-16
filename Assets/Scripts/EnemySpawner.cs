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

    [Header("Health Bar")]
    [SerializeField] GameObject _enemyHealthBarPrefab;

    void Start()
    {
        SetEnemySpawnPositions();
        InvokeRepeating(nameof(HandleGameDifficultyIncrease), 1f, 1f);
    }

    void Update()
    {
        HandleEnemySpawning();
    }

    void HandleEnemySpawning()
    {
        _currentCooldown -= Time.deltaTime;

        if (_currentCooldown > Time.time) return;

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

        Enemy enemy = Instantiate(GetRandomEnemyPrefab(), spawnPosition, Quaternion.identity);

        // Spawn health bar for the enemy
        if (_enemyHealthBarPrefab != null && enemy != null)
        {
            GameObject healthBarObj = Instantiate(_enemyHealthBarPrefab);

            EntityHealth entityHealth = enemy.GetComponent<EntityHealth>();

            if (entityHealth != null)
            {
                // Set up the HealthBar component with the entity's health
                HealthBar healthBar = healthBarObj.GetComponent<HealthBar>();
                if (healthBar != null)
                {
                    healthBar.SetEntityHealth(entityHealth);
                }
                
                // Set up the health bar follower to follow the enemy
                HealthBarFollower follower = healthBarObj.GetComponent<HealthBarFollower>();
                if (follower != null)
                {
                    follower.target = enemy.transform;
                }
                
                // Subscribe to death event to destroy health bar
                entityHealth.OnDeath += () => 
                {
                    if (healthBarObj != null)
                    {
                        Destroy(healthBarObj);
                    }
                };
            }
        }
    }

    void HandleGameDifficultyIncrease()
    {
        _spawnCooldown *= _spawnCooldownReductionMultiplier;
    }
}