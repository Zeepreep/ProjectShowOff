using UnityEngine;
using UnityEngine.VFX;

public class CatScript : MonoBehaviour
{
    public VisualEffect smokeEffect;
    public bool catFound;

    [HideInInspector]
    public Quest quest;

    private void Start()
    {
        
    }

    public void CatPhotographed()
    {
        SoundManager.Instance.PlayCatMeow(transform);
        
        Instantiate(smokeEffect, transform.position, Quaternion.identity);
        
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