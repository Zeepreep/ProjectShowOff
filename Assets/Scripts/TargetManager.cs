using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Serialization;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }
    public Camera cam;
    
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
            CatHandler[] cats = FindObjectsOfType<CatHandler>();
            foreach (var cat in cats)
            {
               // var targetRender = cat.gameObject.GetComponent<Renderer>();
                if (IsVisible(cam, cat.gameObject))
                {
                 //   targetRender.material.SetColor("_Color", Color.red);
                    Debug.Log($"Cat {cat.name} from Quest {cat.quest.questName} is visible");
                }
                else
                {
                   // targetRender.material.SetColor("_Color", Color.black);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
