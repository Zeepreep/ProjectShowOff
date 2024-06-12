using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CatMover : MonoBehaviour
{
    // THIS SCRIPT IS OLD AND NOT USED ANYMORE
    
    
    // Make this script a singleton, this is done to make sure there is only one instance of this object in the scene.
    public static CatMover Instance { get; private set; }

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


    public List<Transform> spawnPoints;
    public VisualEffect smokeEffect;

    public void MoveCat()
    {
        Transform nextPos = spawnPoints[Random.Range(0, spawnPoints.Count)];

        if (nextPos.transform.position == transform.position)
            SoundManager.Instance.PlayCatMeow(transform);

        if (spawnPoints != null)
        {
            if (nextPos.transform.position == transform.position)
            {
                Debug.Log("The cat is already at that position!");
                MoveCat();
            }
            else
            {
                Instantiate(smokeEffect, transform.position, Quaternion.identity);

                transform.position = nextPos.position;
                transform.rotation = nextPos.rotation;
            }
        }


        Debug.Log("Moving Cat");
    }
}