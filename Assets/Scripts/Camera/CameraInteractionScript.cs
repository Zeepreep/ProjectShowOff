using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CameraInteractionScript : MonoBehaviour
{
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
