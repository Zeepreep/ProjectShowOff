using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpawner : MonoBehaviour
{

    public static CatSpawner Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Another instance of this object already exists!");
            Destroy(gameObject);
        }

    }


    public List<Transform> spawnPoints; // List of spawn points (buildings)
    public GameObject catPrefab; // The cat prefab to spawn

    public GameObject currentCat;

    public Vector3 catPos;

    void Start()
    {
        SpawnCat();

        catPos = transform.position;
    }



    public void SpawnCat()
    {
       /* if (spawnPoints.Count == 0 || catPrefab == null)
        {
            Debug.LogWarning("Spawn points list or cat prefab is not set.");
            return;
        }

        // Check if a cat already exists and deactivate it instead of destroying it
       *//* if (currentCat != null)
        {
            currentCat.SetActive(false);
        }*//*

        int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomIndex];
*/
        // Instantiate a new cat at the chosen spawn point
        currentCat = Instantiate(catPrefab, catPos, this.transform.rotation);
        catPos += new Vector3(0, 1, 0);
        Debug.Log("Printing Cat");
    }
}
