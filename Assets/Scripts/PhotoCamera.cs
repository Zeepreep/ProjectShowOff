using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class PhotoCamera : MonoBehaviour
{
    public GameObject photoPrefab;
    public MeshRenderer screenRenderer;
    public Transform photoSpawnPosition;
    public TextMeshProUGUI detectionText; // Reference to the text UI element for detection status

    private Camera photoCamera;


    [SerializeField]
    CatSpawner catSpawner;

    public bool catPhotographed;


    private void Awake()
    {
        photoCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        StartCoroutine(CheckZoom());

        CreateRenderTexture();
        TurnOn();
    }

    public void CreateRenderTexture()
    {
        RenderTexture newTexture = new RenderTexture(256, 256, 32, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
        newTexture.antiAliasing = 4;

        photoCamera.targetTexture = newTexture;
        screenRenderer.material.mainTexture = newTexture;
    }

    public void TakePhoto()
    {
        Photo newPhoto = CreatePhoto();
        SetPhotoImage(newPhoto);
        SetDetectionStatus(newPhoto);

        catSpawner.SpawnCat();
    }

    private Photo CreatePhoto()
    {
        Quaternion rotationOffset = Quaternion.Euler(0, 0, 180);
        GameObject photoObject = Instantiate(photoPrefab, photoSpawnPosition.position, photoSpawnPosition.rotation * rotationOffset, transform);
        return photoObject.GetComponent<Photo>();
    }

    void SetPhotoImage(Photo photo)
    {
        Texture2D newTexture = RenderCameraToTexture(photoCamera);
        if (photo != null)
        {
            photo.SetImage(newTexture);
        }
        else
        {
            Debug.Log("No Photo object exists!");
        }
    }

    private Texture2D RenderCameraToTexture(Camera camera)
    {
        camera.Render();
        RenderTexture.active = camera.targetTexture;

        Texture2D photo = new Texture2D(256, 256, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        photo.Apply();

        return photo;
    }

    private void SetDetectionStatus(Photo photo)
    {
        if (TargerManagement.Instance.IsVisible(TargerManagement.Instance.cam, TargerManagement.Instance.target))
        {
            detectionText.text = ("Cat visible!");
            Debug.Log("Cat visible");

            catPhotographed = true;
        }
        else
        {
           detectionText.text = ("Cat not visible!");
            Debug.Log("Cat not visible");
        }
    }

    public void TurnOn()
    {
        photoCamera.enabled = true;
        screenRenderer.material.color = Color.white;
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

                    photoCamera.fieldOfView += zoomInput * zoomSpeed * Time.deltaTime;
                    photoCamera.fieldOfView = Mathf.Clamp(photoCamera.fieldOfView, minFov, maxFox);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}