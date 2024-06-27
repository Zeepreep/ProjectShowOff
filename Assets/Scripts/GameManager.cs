using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Header Debugging")]
    public bool hideOtherLevelCats;
    public bool autoChangeLevels;

    [Header("Level Inputs")]
    public GameObject LevelAssets;

    [Header("GameObject Inputs")]
    public GameObject TutorialText;
    public GameObject PhotoSpots;
    public GameObject[] LevelPositions;
    
    [Header("Board Inputs")]
    public TextMeshProUGUI level1CompleteText;
    public TextMeshProUGUI level2CompleteText;
    public TextMeshProUGUI level3CompleteText;
    
    [Header("Other Stuff")]

    public int currentLevel;

    [HideInInspector] public List<CatScript> cats;

    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;

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
        // Set initial alpha to 0 (not dark at the start)
        fadeCanvasGroup.alpha = 0;

        if (!TutorialText.activeSelf)
        {
            TutorialText.SetActive(true);
        }

        if (PhotoSpots.activeSelf)
        {
            PhotoSpots.SetActive(false);
        }

        StartCoroutine(ActivateCats());
        StartCoroutine(levelCompleteStatus());
        PopulateLists();

        if (autoChangeLevels)
        {
            StartCoroutine(AutoChangeLevel());
        }
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
                        if (cat.catCorrespondingLevel == currentLevel)
                        {
                            if (!cat.gameObject.activeSelf)
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
            if (cat.quest != null && cat.catCorrespondingLevel == currentLevel)
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
            StartCoroutine(FadeAndLoadNextLevel());
        }
        else
        {
            SoundManager.Instance.PlayCatMeow();
            Debug.Log("Not all cats have been found in this level.");
        }
    }

    private IEnumerator levelCompleteStatus()
    {
        while (true)
        {
            if (IsLevelCompleted())
            {
                switch (currentLevel)
                {
                    case 1:
                        level1CompleteText.text = "Level 1 Complete!";
                        break;
                    case 2:
                        level2CompleteText.text = "Level 2 Complete!";
                        break;
                    case 3:
                        level3CompleteText.text = "Level 3 Complete!";
                        break;
                }
            }
            else
            {
                switch (currentLevel)
                {
                    case 1:
                        level1CompleteText.text = "Level 1 Incomplete";
                        break;
                    case 2:
                        level2CompleteText.text = "Level 2 Incomplete";
                        break;
                    case 3:
                        level3CompleteText.text = "Level 3 Incomplete";
                        break;
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Going to next level through Key Press");
            StartCoroutine(FadeAndLoadNextLevel());
        }
    }

    private IEnumerator FadeAndLoadNextLevel()
    {
        // Play the audio level transition sound
        // SoundManager.Instance.PlayLevelTransition();

        if (currentLevel == 0)
        {
            Debug.Log("PhotoSpots activated");
            TutorialText.SetActive(false);
            PhotoSpots.SetActive(true);
            SoundManager.Instance.PlayButtonClick();

            currentLevel++;
        }
        else
        {
            // Apply fade effect only if transitioning between levels 1 and 3
            if (currentLevel >= 1 && currentLevel < 3)
            {
                yield return StartCoroutine(Fade(0, 1)); // Fade from current alpha to white
            }

            currentLevel++;

            if (currentLevel >= LevelPositions.Length)
            {
                Debug.LogWarning("No more levels available");
            }
            else
            {
                Debug.Log("Level " + currentLevel + " activated");

                LevelAssets.transform.parent = LevelPositions[currentLevel].transform;
                LevelAssets.transform.localPosition = Vector3.zero;
                LevelAssets.transform.localRotation = Quaternion.identity;
            }

            SoundManager.Instance.PlayButtonClick();

            // Apply fade effect only if transitioning between levels 1 and 3
            if (currentLevel > 1 && currentLevel <= 3)
            {
                yield return StartCoroutine(Fade(1, 0)); // Fade from white to dark
            }
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
    }
}