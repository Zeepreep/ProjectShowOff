using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Transform[] waypoints;

    void Start()
    {
      //  InitializeWaypoints();
    }

    void InitializeWaypoints()
    {
        // Automatically find all waypoints in the scene
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new Transform[waypointObjects.Length];
        for (int i = 0; i < waypointObjects.Length; i++)
        {
            waypoints[i] = waypointObjects[i].transform;
        }
    }
}
