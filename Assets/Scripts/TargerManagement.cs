using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class TargerManagement : MonoBehaviour
{
    public static TargerManagement Instance { get; private set; }
    public CatSpawner catSpawner;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Another instance of this object already exists!");
            Destroy(gameObject);
        }

    }

    public GameObject target;
    public Camera cam;


    void Start()
    {
        StartCoroutine(IsCatSeen());
    }

    public bool IsVisible(Camera c, GameObject target)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var point = target.transform.position;

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        return true;
    }

   
    IEnumerator IsCatSeen()
    {
        while (true)
        {
            var targetRender = target.GetComponentInChildren<Renderer>();
            if (IsVisible(cam, target))
            {
                targetRender.material.SetColor("_Color", Color.red);
            }
            else
            {
                targetRender.material.SetColor("_Color", Color.black);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
