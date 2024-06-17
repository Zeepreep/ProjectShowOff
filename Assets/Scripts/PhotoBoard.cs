using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoBoard : MonoBehaviour
{
    public static PhotoBoard Instance;
    
    //debug line
    
    public Quests quests;

    [Header("Debug")] public List<PhotoSpot> photoSpots;

    private void Awake()
    {
        // photoSpots = new List<PhotoSpot>(FindObjectsOfType<PhotoSpot>());
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one PhotoBoard in the scene");
            Destroy(this);
        }
    }
}