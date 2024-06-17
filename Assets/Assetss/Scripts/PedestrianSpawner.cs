using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public GameObject[] pedestrianPrefabs; // Array to hold multiple pedestrian prefabs
    public float spawnInterval = 5.0f;
    public int maxPedestrians = 10; // Maximum number of pedestrians to spawn
    private float timer;
    private int currentPedestrianCount = 0; // Current number of spawned pedestrians

    public WaypointManager waypointManager; // Assign the appropriate WaypointManager

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

        // Ensure the prefab is not affecting the camera
        GameObject newPedestrian = Instantiate(pedestrianPrefab, transform.position, Quaternion.identity);
        newPedestrian.name = "Pedestrian_" + currentPedestrianCount;

        // Ensure the new pedestrian does not have a camera component
        Camera pedestrianCamera = newPedestrian.GetComponentInChildren<Camera>();
        if (pedestrianCamera != null)
        {
            Destroy(pedestrianCamera.gameObject);
//            Debug.LogWarning("Removed unintended camera from pedestrian prefab.");
        }

        // Assign the WaypointManager to the new pedestrian
        Pedestrian pedestrianScript = newPedestrian.GetComponent<Pedestrian>();
        if (pedestrianScript != null)
        {
            pedestrianScript.waypointManager = waypointManager;
        }

        // Increase the current pedestrian count
        currentPedestrianCount++;
    }
}
