using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Photo : MonoBehaviour
{
    public MeshRenderer imageRenderer = null;

    private Collider currentCollider = null;
    private ApplyPhysics applyPhysics = null;

    private XRGrabInteractable grabInteractable;

    private bool isFirstPickup;

    private void Awake()
    {
        currentCollider = GetComponent<Collider>();
        applyPhysics = GetComponent<ApplyPhysics>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        isFirstPickup = false;
    }

    private void Start()
    {
        StartCoroutine(EjectOverSeconds(1.5f));
    }

    public IEnumerator EjectOverSeconds(float seconds)
    {
        //DisablePhysics();
        currentCollider.enabled = false;

        float elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            transform.position += transform.forward * Time.deltaTime * 0.1f;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        currentCollider.enabled = true;
    }

    public IEnumerator CheckIfPickedUp()
    {
        while (isFirstPickup)
        {
            if (grabInteractable.isSelected)
            {
                EnablePhysics();

                isFirstPickup = true;

                Debug.Log("Picked up");
            }

            Debug.Log("Not picked up");

            yield return null;
        }
    }


    public void SetImage(Texture2D texture)
    {
        imageRenderer.material.color = Color.white;
        imageRenderer.material.mainTexture = texture;
    }

   

    public void EnablePhysics()
    {
        applyPhysics.ApplyPhysicsToRigidbody();
        transform.parent = null;
    }

    public void DisablePhysics()
    {
        applyPhysics.DisablePhysicsToRigidbody();
    }
}
