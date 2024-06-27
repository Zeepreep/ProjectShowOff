using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotoCamera : MonoBehaviour
{
    [Header("Photo Camera Inserts")]
    public GameObject photoPrefab;
    public MeshRenderer screenRenderer;
    public Transform photoSpawnPosition;
    public Transform cameraSpawnPosition;

    private Camera photoCamera;
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private float zoomSpeed = 30f;
    private float minFov = 0f;

    private float maxFov = 180f;
    private int cameraWidth = 128;
    private int cameraHeight = 64;

    void Awake()
    {
        photoCamera = GetComponentInChildren<Camera>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        if (grabInteractable == null)
        {
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
        }
        grabInteractable.selectExited.AddListener(OnCameraDropped);

        CreateRenderTexture();
    }

    void Start()
    {
        StartCoroutine(CheckZoom());
    }

    private void OnCameraDropped(SelectExitEventArgs arg)
    {
        StartCoroutine(MoveCameraToSpawn());
    }

    IEnumerator MoveCameraToSpawn()
    {
        float duration = 1.0f;  // Duration over which the camera will return to its position
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;

            // Interpolate position and rotation back to spawn point
            transform.position = Vector3.Lerp(startPosition, cameraSpawnPosition.position, normalizedTime);
            transform.rotation = Quaternion.Lerp(startRotation, cameraSpawnPosition.rotation, normalizedTime);

            yield return null;
        }

        transform.position = cameraSpawnPosition.position;
        transform.rotation = cameraSpawnPosition.rotation;
    }

    public IEnumerator CheckZoom()
    {
        while (true)
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDeviceCharacteristics rightControllerCharacteristics =
                InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);

            if (devices.Count > 0)
            {
                InputDevice device = devices[0];
                Vector2 input;
                if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out input))
                {
                    float zoomInput = input.y;

                    photoCamera.fieldOfView -= zoomInput * zoomSpeed * Time.deltaTime;
                    photoCamera.fieldOfView = Mathf.Clamp(photoCamera.fieldOfView, minFov, maxFov);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
    private void CreateRenderTexture()
    {
        RenderTexture renderTexture = new RenderTexture(cameraWidth, cameraHeight, 24);
        photoCamera.targetTexture = renderTexture;
        screenRenderer.material.mainTexture = renderTexture;
    }

    public void TakePhoto()
    {
        Photo newPhoto = CreatePhoto();
        SetPhotoImage(newPhoto);
        //UpdateDebugText(newPhoto);

        // Check if the pictured cat is associated with an active quest
        CatScript picturedCat = GetPicturedCat();
        if (picturedCat != null && picturedCat.quest != null)
        {
            picturedCat.CatPhotographed();

            picturedCat.quest.isCompleted = true;
            Debug.Log($"Quest {picturedCat.quest.questName} is completed");
        }
        else
        {
            Debug.Log("No active quest for this cat");
        }

        SoundManager.Instance.PlayCameraShutter(transform);
    }

    /// <summary>
    /// Instantiate the actual photo object.
    /// </summary>
    /// <returns>Photo object</returns>
    private Photo CreatePhoto()
    {
        Quaternion rotationOffset = Quaternion.Euler(0, 0, 180);
        GameObject photoObject = Instantiate(photoPrefab, photoSpawnPosition.position,
            photoSpawnPosition.rotation * rotationOffset, transform);
        return photoObject.GetComponent<Photo>();
    }

    /// <summary>
    /// Creates the texture to be set on the Photo object.
    /// </summary>
    void SetPhotoImage(Photo photo)
    {
        Texture2D newTexture = RenderCameraToTexture(photoCamera);
        if (photo != null)
        {
            photo.SetImage(newTexture, GetPicturedCat());
        }
        else
        {
            Debug.Log("No Photo object exists or no cat was in the picture!");
        }
    }

    /// <summary>
    /// Reads the pixels from the viewfinder camera and prints them to a Texture2D.
    /// </summary>
    /// <returns>Texture2D of the photo.</returns>
    private Texture2D RenderCameraToTexture(Camera camera)
    {
        camera.Render();
        RenderTexture.active = camera.targetTexture;

        Texture2D photo = new Texture2D(cameraWidth / 2, cameraHeight, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(cameraHeight / 2, 0, cameraWidth / 2, cameraHeight), 0, 0);
        photo.Apply();

        return photo;
    }

    /// <summary>
    /// Used to check if any Cat is currently in the viewfinder camera.
    /// </summary>
    /// <returns>cat</returns>
    private CatScript GetPicturedCat()
    {
        CatScript[] cats = FindObjectsOfType<CatScript>();
        foreach (var cat in cats)
        {
            if (TargetManager.Instance.IsVisible(TargetManager.Instance.cam, cat.gameObject))
            {
                return cat;
            }
        }

        Debug.Log("No cat in viewfinder!");
        return null;
    }

    /// <summary>
    /// Updates the debug text that displays if the cat is visible or not.
    /// </summary>
    /// <param name="photo"></param>
    private void UpdateDebugText(Photo photo)
    {
        if (photo.catPictured)
        {
            Debug.Log("Cat visible");
        }
        else
        {
            Debug.Log("Cat not visible");
        }
    }
}