using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class PickupSpawnInfo
{
    public GameObject pickupPrefab;
    public int maxToSpawn = 10;
}

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] PickupSpawnInfo[] _pickupSpawnInfos;
    [SerializeField] Tilemap _pickupSpawnTiles;

    void Awake()
    {
        SpawnPickups();
    }

    void SpawnPickups()
    {
        List<Vector3> spawnPositions = GetSpawnPositions();

        if (spawnPositions.Count == 0)
        {
            Debug.LogError("No pickup spawn positions found! Make sure PickupSpawn tiles are placed.");
            return;
        }

        List<Vector3> availablePositions = new List<Vector3>(spawnPositions);
        int totalSpawned = 0;

        foreach (PickupSpawnInfo spawnInfo in _pickupSpawnInfos)
        {
            if (spawnInfo.pickupPrefab == null)
            {
                Debug.LogWarning("Pickup prefab is null in spawn info, skipping.");
                continue;
            }

            int amountToSpawn = Mathf.Min(spawnInfo.maxToSpawn, availablePositions.Count);

            for (int i = 0; i < amountToSpawn; i++)
            {
                Vector3 spawnPosition;

                int randomIndex = Random.Range(0, availablePositions.Count);
                spawnPosition = availablePositions[randomIndex];
                availablePositions.RemoveAt(randomIndex);

                Instantiate(spawnInfo.pickupPrefab, spawnPosition, Quaternion.identity, transform);
                totalSpawned++;
            }

            if (availablePositions.Count == 0)
            {
                Debug.LogWarning("Ran out of spawn positions before spawning all pickups.");
                break;
            }
        }
    }

    List<Vector3> GetSpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();

        foreach (Vector3Int position in _pickupSpawnTiles.cellBounds.allPositionsWithin)
        {
            if (_pickupSpawnTiles.HasTile(position))
            {
                positions.Add(_pickupSpawnTiles.GetCellCenterWorld(position));
            }
        }

        return positions;
    }
}
