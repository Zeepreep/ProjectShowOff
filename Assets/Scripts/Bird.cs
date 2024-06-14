using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed = 1.0f;
    public float boundary = 30.0f;
    
    [HideInInspector]
    public Vector3 direction = Vector3.forward;


    void Update()
    {
        // Move the bird
        transform.Translate(direction * speed * Time.deltaTime);

        // Check if the bird is out of bounds
        if (transform.position.magnitude > boundary)
        {
            // Destroy the bird
            Destroy(gameObject);    
        }
    }
}