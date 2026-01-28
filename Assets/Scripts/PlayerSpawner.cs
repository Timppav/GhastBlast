using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Tilemap _playerSpawnTiles;

    [Header("Spawn Warning")]
    [SerializeField] GameObject _spawnWarningPrefab;
    [SerializeField] float _warningDuration = 1f;

    GameObject _player;

    void Awake()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        List<Vector3> spawnPositions = GetSpawnPositions();

        if (spawnPositions.Count == 0)
        {
            Debug.LogError("No player spawn positions found! Make sure PlayerSpawn tiles are placed.");
            return;
        }

        Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)];

        _player = Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);

        StartCoroutine(PlaySpawnWarningAndEnablePlayer(spawnPosition));

        EnablePlayerComponents(false);
    }

    IEnumerator PlaySpawnWarningAndEnablePlayer(Vector3 spawnPosition)
    {
        GameObject warning = null;
        if (_spawnWarningPrefab != null)
        {
            warning = Instantiate(_spawnWarningPrefab, spawnPosition, Quaternion.identity);
        }

        yield return new WaitForSeconds(_warningDuration);

        if (warning != null)
        {
            Destroy(warning);
        }

        EnablePlayerComponents(true);
    }

    void EnablePlayerComponents(bool active)
    {
        var playerController = _player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = active;
        }

        var spriteRenderer = _player.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = active;
        }

        var staff = _player.transform.Find("Staff");
        if (staff != null)
        {
            staff.gameObject.SetActive(active);
        }
    }

    List<Vector3> GetSpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();

        foreach (Vector3Int position in _playerSpawnTiles.cellBounds.allPositionsWithin)
        {
            if (_playerSpawnTiles.HasTile(position))
            {
                positions.Add(_playerSpawnTiles.GetCellCenterWorld(position));
            }
        }

        return positions;
    }
}
