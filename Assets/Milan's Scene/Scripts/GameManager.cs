using System;
using System.Collections;
using System.Collections.Generic;
using Management;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Header Debugging")] public bool hideOtherLevelCats;

    [Header("Level Inputs")] public GameObject LevelAssets;

    public GameObject[] LevelPositions;

    public int currentLevel;

    [HideInInspector] public List<CatScript> cats;

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
        StartCoroutine(ActivateCats());
        PopulateLists();
        //StartCoroutine(AutoChangeLevel());
    }

    void PopulateLists()
    {
        cats = new List<CatScript>(FindObjectsOfType<CatScript>());

        foreach (CatScript cat in cats)
        {
            if (cat.quest != null)
            {
                cat.quest.questCat = cat;
            }
        }
    }

    public IEnumerator ActivateCats()
    {
        while (true)
        {
            if (hideOtherLevelCats)
            {
                if (cats == null)
                {
                    Debug.LogWarning("No Cats found");
                    yield return null;
                }
                else
                {
                    foreach (CatScript cat in cats)
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
            }

            yield return new WaitForSeconds(1);
        }
    }

    public bool IsLevelCompleted()
    {
        foreach (CatScript cat in cats)
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


    IEnumerator AutoChangeLevel()
    {
        while (true)
        {
            if (IsLevelCompleted())
            {
                currentLevel++;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }

    public void NextLevelButton()
    {
        if (IsLevelCompleted())
        {
            currentLevel++;

            if (currentLevel >= LevelPositions.Length)
            {
                Debug.LogWarning("No more levels available");
            }
            else
            {
                LevelAssets.transform.parent = LevelPositions[currentLevel].transform;
                LevelAssets.transform.localPosition = Vector3.zero;
                LevelAssets.transform.localRotation = Quaternion.identity;

                Debug.Log("Next Level: " + currentLevel);
            }

            SoundManager.Instance.PlayButtonClick();
        }
        else
        {
            SoundManager.Instance.PlayConstructionSounds();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Going to next level through Key Press");
            NextLevelButton();
        }
    }
}