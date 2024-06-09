using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // The prefab to spawn
    public Transform spawnCenter;    // The central point for spawning
    public float spawnRadius = 5f;   // Radius around the central point to spawn
    public int numberOfSpawns = 10;  // Number of objects to spawn

    void Start()
    {
        SpawnAroundPoint();
    }

    void SpawnAroundPoint()
    {
        for (int i = 0; i < numberOfSpawns; i++)
        {
            Vector3 spawnPosition = GenerateRandomPosition();
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GenerateRandomPosition()
    {
        // Generate a random point within a circle
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        return new Vector3(spawnCenter.position.x + randomPoint.x, spawnCenter.position.y, spawnCenter.position.z + randomPoint.y);
    }
}
