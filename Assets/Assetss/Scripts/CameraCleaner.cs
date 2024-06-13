using UnityEngine;

public class CameraCleaner : MonoBehaviour
{
    public GameObject[] objectsToClean;

    void Start()
    {
        foreach (GameObject obj in objectsToClean)
        {
            RemoveUnintendedCameras(obj);
        }
    }

    void RemoveUnintendedCameras(GameObject obj)
    {
        Camera[] cameras = obj.GetComponentsInChildren<Camera>();
        foreach (Camera cam in cameras)
        {
            if (!IsXRCamera(cam))
            {
                Destroy(cam.gameObject);
                Debug.LogWarning("Removed unintended camera from object: " + obj.name);
            }
        }
    }

    bool IsXRCamera(Camera cam)
    {
        // Check if the camera's name contains "XR"
        return cam.name.Contains("XR");
    }
}
