using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker : Pedestrian
{
    protected override void Start()
    {
        base.Start();
        // Additional initialization for Joker if needed
    }

    protected override void Update()
    {
        base.Update();
        // Additional update logic for Joker if needed
    }

    protected override Vector3 GetForwardFacingRotation()
    {
        // Example rotation adjustment for Joker
        return new Vector3(0, 270, 0); // Adjust as needed
    }
}