using System.Collections;
using System.Collections.Generic;
using Management;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel;

    public List<CatHandler> cats = new List<CatHandler>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning("More than one GameSettingsApplier in the scene");
            Destroy(this);
        }
    }

    void Start()
    {
        StartCoroutine(CheckLevel());
        PopulateCatList();
        StartCoroutine(LevelEnder());
    }

    void PopulateCatList()
    {
        cats = new List<CatHandler>(FindObjectsOfType<CatHandler>());
    }

    public IEnumerator CheckLevel()
    {
        while (true)
        {
            if (cats == null)
            {
                Debug.LogWarning("No Cats found");
                yield return null;
            }
            else
            {
                Debug.Log("Cats: " + cats.Count);

                foreach (CatHandler cat in cats)
                {
                    if (cat.quest.correspondingLevel == currentLevel)
                    {
                        if (cat.gameObject.activeSelf == false)
                        {
                            cat.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (cat.gameObject.activeSelf)
                        {
                            cat.gameObject.SetActive(false);
                        }
                    }
                }
            }


            yield return new WaitForSeconds(1);
        }
    }

    public bool AreAllQuestsCompleted()
    {
        foreach (CatHandler cat in cats)
        {
            if (cat.quest != null && cat.quest.correspondingLevel == currentLevel)
            {
                if (!cat.quest.isCompleted)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }

    IEnumerator LevelEnder()
    {
        while (true)
        {
            if (AreAllQuestsCompleted())
            {
                Debug.Log("All quests completed");
                currentLevel++;
                StartCoroutine(CheckLevel());
            }
            
            yield return new WaitForSeconds(0.2f);
        }
    }
}