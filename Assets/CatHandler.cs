using System;
using System.Collections;
using System.Collections.Generic;
using Management;
using UnityEngine;
using UnityEngine.VFX;

public class CatHandler : MonoBehaviour
{
    public VisualEffect smokeEffect;
    public bool catFound;

    [HideInInspector]
    public Quest quest;

    private void Start()
    {
        StartCoroutine(CatFound());
    }

    public void CatPhotographed()
    {
        SoundManager.Instance.PlayCatMeow(transform);
        
        Instantiate(smokeEffect, transform.position, Quaternion.identity);
        
        if (quest == null)
        {
            Debug.LogWarning("Quest is not assigned to the cat"); 
        }
        else
        {
            quest.questPhotoSpot.questCompletedText.text = "Completed";
        }
        
        
        // debug log the name of the object and the text "Cat Photographed"
        Debug.Log("Cat Photographed: " + name);
    }

    public IEnumerator CatFound()
    {
        while (true)
        {
            if (catFound)
            {
                Debug.Log("Cat Found: " + name);

                quest.isCompleted = true;
            }
            
            yield return new WaitForEndOfFrame();
        }
    }

}