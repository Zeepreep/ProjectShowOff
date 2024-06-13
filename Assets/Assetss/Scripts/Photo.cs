using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        StartCoroutine(CheckIfPickedUp());
    }

    public IEnumerator EjectOverSeconds(float seconds)
    {
       // DisablePhysics();
        SoundManager.Instance.PlayPhotoPrint(transform);
        
        currentCollider.enabled = false;

        yield return new WaitForSeconds(0.5f);

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