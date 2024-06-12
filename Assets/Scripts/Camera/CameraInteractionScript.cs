using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CameraInteractionScript : MonoBehaviour
{
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