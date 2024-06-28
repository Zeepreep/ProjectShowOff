using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class CatScript : MonoBehaviour
{
    public VisualEffect smokeEffect;
    public bool catFound;

    [HideInInspector] public Quest quest;
    
    public int catCorrespondingLevel;
    
    public void CatPhotographed()
    {
        SoundManager.Instance.PlayCatMeow(transform);

        VisualEffect poof = Instantiate(smokeEffect, transform.position, Quaternion.identity);
        Destroy(poof, 2f);
        

        if (quest != null)
        {
            quest.isCompleted = true;
        }
        else
        {
            Debug.LogWarning("Quest is not assigned to the cat");
        }


        // debug log the name of the object and the text "Cat Photographed"
        Debug.Log("Cat Photographed: " + name);
    }
}