using System.Collections;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public GameObject birdPrefab;
    public float spawnRate = 1.0f;
    public float maxHeight = 100.0f;
    public float minHeight = 50.0f;
    public float spawnRadius = 300.0f;

    public float maxBirdsAmount;

    private GameObject birdParent;
    
    void Start()
    {
        birdParent = new GameObject("Birds");
        
        // Start spawning birds
        StartCoroutine(SpawnBirds());
    }

    IEnumerator SpawnBirds()
    {
        while (birdParent.transform.childCount < maxBirdsAmount)
        {
            yield return new WaitForSeconds(1.0f / spawnRate);

            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(minHeight, maxHeight),
                Random.Range(-spawnRadius, spawnRadius)
            );
            
            Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            // Instantiate a new bird
            Instantiate(birdPrefab, spawnPosition, spawnRotation, birdParent.transform);
        }
    }
}