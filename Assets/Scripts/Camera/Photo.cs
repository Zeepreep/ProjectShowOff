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
    private bool isPhotoTrashed;
    private Collider lastCollidedObject;

    private Texture2D photographDisplayed;


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
        SoundManager.Instance.PlayPhotoPrint(transform);

        currentCollider.enabled = false;

        float elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            transform.position += transform.forward * Time.deltaTime * 0.21f;
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
        photographDisplayed = texture;
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
                    
                 //   Debug.Log("Highlighting sphere");
                }
                else
                {
                    Debug.Log("Renderer is null");
                }

                if (catPictured != null && catPictured == photoSpot.quest.questCat)
                {
                    isPhotoHangable = true;
                    sphereRenderer.material = highlightMaterial;
                    
               //     Debug.Log("Photo is hangable");
                }
                else
                {
                    isPhotoHangable = false;
                    sphereRenderer.material = denyMaterial;
                }
                
            }
            else if (Physics.OverlapSphere(transform.position, pictureOverlapSize,
                         LayerMask.GetMask("TrashSpots")).Length > 0)
            {
                lastCollidedObject = Physics.OverlapSphere(transform.position, pictureOverlapSize,
                    LayerMask.GetMask("TrashSpots"))[0];

                Renderer sphereRenderer = lastCollidedObject.gameObject.GetComponent<Renderer>();

                isPhotoTrashed = true;

                sphereRenderer.material = denyMaterial;
                
             //   Debug.Log("Photo is trashed");
            }
            else
            {
                if (lastCollidedObject != null)
                {
                    Renderer renderer = lastCollidedObject.gameObject.GetComponent<Renderer>();
                    renderer.material = nonHighlightMaterial;
                }

                isPhotoHangable = false;
                isPhotoTrashed = false;
                
//                Debug.Log("Photo is not hangable or trashed");
            }


            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator ShredderMovement()
    {
        SoundManager.Instance.PlayConstructionSounds(transform);

        float elapsedTime = 0;
        Vector3 startPosition = 
            new Vector3(transform.position.x, transform.position.y + 0.04f, transform.position.z); 
        Vector3 endPosition =
            new Vector3(transform.position.x, transform.position.y - 0.16f, transform.position.z); 

        while (elapsedTime < 1f) 
        {
            transform.position =
                Vector3.Lerp(startPosition, endPosition, elapsedTime / 2f); 
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f); // Wait for 2 seconds

        Destroy(gameObject);
    }

    private void OnReleased()
    {
        Debug.Log("Let Photograph Go");
        
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 hangOffset = new Vector3(90, 180, 0);

        transform.parent = null;

        if (isPhotoTrashed)
        {
            transform.position = lastCollidedObject.transform.position;
            transform.rotation = lastCollidedObject.transform.rotation * Quaternion.Euler(hangOffset);
            
            SoundManager.Instance.PlayPaperShredding(transform);
            
            StartCoroutine(ShredderMovement());
        }
        else if (isPhotoHangable)
        {
            BoxCollider currentCollider = GetComponent<BoxCollider>();
            currentCollider.enabled = false;

            transform.position = lastCollidedObject.transform.position;
            transform.rotation = lastCollidedObject.transform.rotation * Quaternion.Euler(hangOffset);
            
            SoundManager.Instance.PlayPictureHung(transform);

            Debug.Log(lastCollidedObject.name);

            lastCollidedObject.GameObject().GetComponentInParent<PhotoSpot>().quest.isCompleted = true;
            lastCollidedObject.GameObject().GetComponentInParent<PhotoSpot>().CreateTexts();

            lastCollidedObject.GameObject().GetComponentInParent<PhotoSpot>().photoSphere.SetActive(false);
            
            transform.parent = lastCollidedObject.transform.parent;

            catPictured.quest.questPhoto = photographDisplayed;
        }
        else
        {
            Debug.Log("nothing to do with photo, doing nothing");
        }

        rb.useGravity = true;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}