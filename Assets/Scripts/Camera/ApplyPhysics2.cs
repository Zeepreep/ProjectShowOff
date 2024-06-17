using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ApplyPhysics2 : MonoBehaviour
{
    private Rigidbody rb;
    
    public void ApplyPhysicsToRigidbody()
    {
        rb.isKinematic = false;
    }
    
    public void DisablePhysicsToRigidbody()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
}
