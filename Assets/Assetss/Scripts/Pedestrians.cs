using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    public float speed = 2.0f;
    public float rotationSpeed = 5.0f;
    protected Transform targetWaypoint;
    protected int targetWaypointIndex = 0;
    protected WaypointManager waypointManager;

    protected virtual void Start()
    {
        waypointManager = FindObjectOfType<WaypointManager>();
        if (waypointManager != null && waypointManager.waypoints.Length > 0)
        {
            targetWaypoint = waypointManager.waypoints[targetWaypointIndex];
        }
    }

    protected virtual void Update()
    {
        if (targetWaypoint != null)
        {
            MoveTowardsTarget();
        }
    }

    protected void MoveTowardsTarget()
    {
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        float step = speed * Time.deltaTime;

        // Move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

        // Rotate towards the target
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion adjustedRotation = targetRotation * Quaternion.Euler(GetForwardFacingRotation());
        transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, rotationSpeed * Time.deltaTime);

        // Check if the pedestrian is close to the target waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            targetWaypointIndex = (targetWaypointIndex + 1) % waypointManager.waypoints.Length;
            targetWaypoint = waypointManager.waypoints[targetWaypointIndex];
        }
    }

    protected virtual Vector3 GetForwardFacingRotation()
    {
        return Vector3.zero;
    }
}
