using System.Collections.Generic;
using UnityEngine;

public class RoadPoint : MonoBehaviour
{
    public enum PointType
    {
        Normal,
        StoppingPoint
    }

    public PointType pointType;
    public float speed;

    public List<RoadPoint> connectedPoints = new List<RoadPoint>(); // List to store connected points

    // Method to add a connected point
    public void AddConnectedPoint(RoadPoint point)
    {
        connectedPoints.Add(point);
    }

    // Method to remove a connected point
    public void RemoveConnectedPoint(RoadPoint point)
    {
        connectedPoints.Remove(point);
    }
}
