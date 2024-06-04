using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ApplyPhysics : MonoBehaviour
{
    private Rigidbody rb;
    
    private void Start()
    {
    }
    
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
