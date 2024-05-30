using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public GameObject[] pedestrianPrefabs; // Array to hold multiple pedestrian prefabs
    public float spawnInterval = 5.0f;
    public int maxPedestrians = 10; // Maximum number of pedestrians to spawn
    private float timer;
    private int currentPedestrianCount = 0; // Current number of spawned pedestrians

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnPedestrian();
            timer = 0;
        }
    }

    void SpawnPedestrian()
    {
        if (currentPedestrianCount >= maxPedestrians)
        {
            Debug.Log("Maximum number of pedestrians reached.");
            return;
        }

        if (pedestrianPrefabs.Length == 0)
        {
            Debug.LogWarning("No pedestrian prefabs assigned!");
            return;
        }

        // Select a random prefab from the array
        int randomIndex = Random.Range(0, pedestrianPrefabs.Length);
        GameObject pedestrianPrefab = pedestrianPrefabs[randomIndex];

        // Instantiate the selected prefab
        Instantiate(pedestrianPrefab, transform.position, Quaternion.identity);

        // Increase the current pedestrian count
        currentPedestrianCount++;
    }
}
