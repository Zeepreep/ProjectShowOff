using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotoCamera : MonoBehaviour
{
    [Header("Photo Camera Inserts")] public GameObject photoPrefab;
    public MeshRenderer screenRenderer;

    public Transform photoSpawnPosition;

    //public TextMeshProUGUI detectionText;
    public Transform cameraSpawnPosition;

    private float collisionSphereSize = 0.30f;

    private Camera photoCamera;
    private float zoomSpeed = 30f;
    private float minFov = 0f;
    private float maxFox = 180f;

    private void Awake()
    {
        photoCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        StartCoroutine(CheckZoom());
        StartCoroutine(DropTest());

        CreateRenderTexture();
    }

    /// <summary>
    /// Create the texture for the viewfinder / the screen on the camera
    /// The photo is extracted from this texture
    /// </summary>
    public void CreateRenderTexture()
    {
        RenderTexture newTexture =
            new RenderTexture(512, 256, 32, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
        newTexture.antiAliasing = 4;

        photoCamera.targetTexture = newTexture;
        screenRenderer.material.mainTexture = newTexture;
    }

    /// <summary>
    /// Creates the Photo object and sets its image, also plays the sound.
    /// Debug text is updated from here too for now.
    /// </summary>
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

        Texture2D photo = new Texture2D(256, 256, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(128, 0, 256, 256), 0, 0);
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

    /// <summary>
    /// Handles the inputs for zooming in and out.
    /// </summary>
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
                    photoCamera.fieldOfView = Mathf.Clamp(photoCamera.fieldOfView, minFov, maxFox);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Checks if the camera is interacting with a building and teleports it back if it is.
    /// </summary>
    IEnumerator DropTest()
    {
        while (true)
        {
            RaycastHit hit;

            if (Physics.OverlapSphere(transform.position, collisionSphereSize,
                    LayerMask.GetMask("Building")).Length > 0)
            {
//                Debug.Log("Camera hit building");
                TeleportCameraBack();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Used to teleport the camera back to its spawn position.
    /// </summary>
    void TeleportCameraBack()
    {
        // Debug.Log("Teleporting camera back");

        Rigidbody rb = GetComponent<Rigidbody>();
        
        transform.SetParent(cameraSpawnPosition);

        transform.position = cameraSpawnPosition.position;
        transform.rotation = cameraSpawnPosition.rotation;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collisionSphereSize);
    }
}