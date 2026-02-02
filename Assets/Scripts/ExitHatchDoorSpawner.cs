using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitHatchDoorSpawner : MonoBehaviour
{
    [SerializeField] GameObject _exitHatchDoorPrefab;
    [SerializeField] Tilemap _doorSpawnTiles;

    void Awake()
    {
        _doorSpawnTiles.GetComponent<TilemapRenderer>().enabled = false;
        SpawnExitDoor();
    }

    void SpawnExitDoor()
    {
        List<Vector3> spawnPositions = GetSpawnPositions();

        if (spawnPositions.Count == 0)
        {
            Debug.LogError("No exit door spawn positions found! Make sure DoorSpawn tiles are placed.");
            return;
        }

        int randomIndex = Random.Range(0, spawnPositions.Count);
        Vector3 spawnPosition = spawnPositions[randomIndex];

        Instantiate(_exitHatchDoorPrefab, spawnPosition, Quaternion.identity, transform);
    }

    List<Vector3> GetSpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();

        foreach (Vector3Int position in _doorSpawnTiles.cellBounds.allPositionsWithin)
        {
            if (_doorSpawnTiles.HasTile(position))
            {
                positions.Add(_doorSpawnTiles.GetCellCenterWorld(position));
            }
        }

        return positions;
    }
}
