using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CameraInteractionScript : MonoBehaviour
{
<<<<<<< Updated upstream:Assets/Scripts/Camera/CameraInteractionScript.cs
   public PhotoCamera PhotoCamera;
   public ParticleSystem particles;
   
   private void Start()
   {
      XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
      grabInteractable.activated.AddListener(x => StartParticles()); 
      grabInteractable.deactivated.AddListener(x => StopParticles());
   }

   private void StartParticles()
   {
      PhotoCamera.TakePhoto();
      particles.Play();
   }

   void StopParticles()
   {
       particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
   }
}
=======
    public PhotoCamera PhotoCamera;
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(x => OnGrabbed());
        grabInteractable.selectExited.AddListener(x => OnReleased());

        
        grabInteractable.activated.AddListener(x => TriggerPressed());
    }

    private void OnGrabbed()
    {
        rb.useGravity = true;
    }
    
   private void OnReleased()
    {
        rb.useGravity = true;
    }
    
    private void TriggerPressed()
    {
        PhotoCamera.TakePhoto();
    }
    
}
>>>>>>> Stashed changes:Assets/Scripts/CameraInteractionScript.cs
