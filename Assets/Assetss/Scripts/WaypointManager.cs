using UnityEngine;
using System.Linq;

public class WaypointManager : MonoBehaviour
{
    public string category;
    public Transform[] waypoints;

    void Start()
    {
        InitializeWaypoints();
    }

    void InitializeWaypoints()
    {
        // Automatically find all waypoints in the scene
        Waypoint[] waypointObjects = GameObject.FindObjectsOfType<Waypoint>();
        // Filter waypoints by category and sort by index
        var filteredWaypoints = waypointObjects.Where(wp => wp.category == category).OrderBy(wp => wp.index).ToArray();

        // Log the filtered waypoints for debugging
        Debug.Log("Filtered waypoints for category " + category + ": " + string.Join(", ", filteredWaypoints.Select(wp => wp.name)));

        // Initialize the waypoints array
        waypoints = new Transform[filteredWaypoints.Length];
        for (int i = 0; i < filteredWaypoints.Length; i++)
        {
            waypoints[i] = filteredWaypoints[i].transform;
        }

        // Log the assigned waypoints for debugging
        Debug.Log("Assigned waypoints for category " + category + ": " + string.Join(", ", waypoints.Select(wp => wp.name)));
    }
}
