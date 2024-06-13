using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Photo : MonoBehaviour
{
    [Header("Photo Inputs")] public MeshRenderer imageRenderer = null;

    [Header("Interaction Materials")] public Material nonHighlightMaterial = null;
    public Material highlightMaterial = null;
    public Material denyMaterial = null;

    [Header("Debug Options")] public CatScript catPictured;

    private Collider currentCollider = null;
    private ApplyPhysics applyPhysics = null;
    private XRGrabInteractable grabInteractable;

    private bool isPhotoHangable;
    private Collider lastCollidedObject;


    private void Awake()
    {
        currentCollider = GetComponent<Collider>();
        applyPhysics = GetComponent<ApplyPhysics>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Start()
    {
        StartCoroutine(EjectOverSeconds(1.5f));
        StartCoroutine(CanBeHung());

        grabInteractable.selectExited.AddListener(x => OnReleased());
    }

    public IEnumerator EjectOverSeconds(float seconds)
    {
        // DisablePhysics();
        SoundManager.Instance.PlayPhotoPrint(transform);

        currentCollider.enabled = false;

        float elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            transform.position += transform.forward * Time.deltaTime * 0.07f;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        currentCollider.enabled = true;
    }

    public void SetImage(Texture2D texture, CatScript pCatPictured)
    {
        imageRenderer.material.color = Color.white;
        imageRenderer.material.mainTexture = texture;

        catPictured = pCatPictured;
    }

    IEnumerator CanBeHung()
    {
        float pictureOverlapSize = 0.05f;

        while (true)
        {
            if (Physics.OverlapSphere(transform.position, pictureOverlapSize,
                    LayerMask.GetMask("PhotoSpots")).Length > 0)
            {
                lastCollidedObject = Physics.OverlapSphere(transform.position, pictureOverlapSize,
                    LayerMask.GetMask("PhotoSpots"))[0];

                Renderer sphereRenderer = lastCollidedObject.gameObject.GetComponent<Renderer>();

                PhotoSpot photoSpot = lastCollidedObject.gameObject.GetComponentInParent<PhotoSpot>();

                if (lastCollidedObject.GameObject().GetComponent<Renderer>() != null)
                {
                    sphereRenderer = lastCollidedObject.GameObject().GetComponent<Renderer>();
                    sphereRenderer.material = highlightMaterial;
                }
                else
                {
                    Debug.Log("Renderer is null");
                }

                if (catPictured != null && catPictured == photoSpot.quest.questCat)
                {
                    isPhotoHangable = true;
                    sphereRenderer.material = highlightMaterial;
                }
                else
                {
                    isPhotoHangable = false;
                    sphereRenderer.material = denyMaterial;
                }
            }
            else
            {
                if (lastCollidedObject != null)
                {
                    Renderer renderer = lastCollidedObject.gameObject.GetComponent<Renderer>();
                    renderer.material = nonHighlightMaterial;
                }

                isPhotoHangable = false;
            }


            yield return new WaitForEndOfFrame();
        }
    }

    private void OnReleased()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        transform.parent = null;

        if (isPhotoHangable)
        {
            BoxCollider currentCollider = GetComponent<BoxCollider>();
            currentCollider.enabled = false;

            transform.position = lastCollidedObject.transform.position;

            Vector3 hangOffset = new Vector3(90, 180, 0);

            transform.rotation = lastCollidedObject.transform.rotation * Quaternion.Euler(hangOffset);

            Debug.Log(lastCollidedObject.name);
            
            lastCollidedObject.GameObject().GetComponentInParent<PhotoSpot>().quest.isCompleted = true;
            lastCollidedObject.GameObject().GetComponentInParent<PhotoSpot>().CreateTexts();
            
            lastCollidedObject.GameObject().GetComponentInParent<PhotoSpot>().photoSphere.SetActive(false);
            
        }

        rb.useGravity = true;
    }
}