using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Newspaper : MonoBehaviour
{
    [Header("Photo Inputs")] 
    public MeshRenderer image1 = null;
    public MeshRenderer image2 = null;
    public MeshRenderer image3 = null;
    
    [Header("Interaction Materials")]
    private Collider currentCollider = null;
    private ApplyPhysics applyPhysics = null;
    private XRGrabInteractable grabInteractable;


    private void Awake()
    {
        currentCollider = GetComponent<Collider>();
        applyPhysics = GetComponent<ApplyPhysics>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }
    
    public void SetImage(Texture2D image1Texture, Texture2D image2Texture, 
                            Texture2D image3Texture)
    {
        image1.material.color = Color.white;
        image1.material.mainTexture = image1Texture;
        
        image2.material.color = Color.white;
        image2.material.mainTexture = image2Texture;
        
        image3.material.color = Color.white;
        image3.material.mainTexture = image3Texture;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}