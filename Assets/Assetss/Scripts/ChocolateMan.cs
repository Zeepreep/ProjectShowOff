using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocolateMan : Pedestrian
{
    protected override void Start()
    {
        base.Start();
        // Additional initialization for Chocolate Man if needed
    }

    protected override void Update()
    {
        base.Update();
        // Additional update logic for Chocolate Man if needed
    }

    protected override Vector3 GetForwardFacingRotation()
    {
        // Example rotation adjustment for Chocolate Man
        return new Vector3(0, 270, 0); // Adjust as needed
    }
}