using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CameraInteractionScript : MonoBehaviour
{
    public PhotoCamera PhotoCamera;
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>(); // Assign the XRGrabInteractable component

        // Add event listeners for XRGrabInteractable events with correct delegate signatures
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
        grabInteractable.activated.AddListener(OnActivated);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        rb.useGravity = true; // Ensure rb is assigned properly in Unity inspector
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        rb.useGravity = true; // Ensure rb is assigned properly in Unity inspector
    }

    private void OnActivated(ActivateEventArgs args)
    {
        PhotoCamera.TakePhoto(); // Call TakePhoto method from PhotoCamera script
    }
}
