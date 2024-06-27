using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public Transform[] CameraSpawnPositions; // Add this line

    [Header("Board Inputs")]
    public TextMeshProUGUI level1CompleteText;
    public TextMeshProUGUI level2CompleteText;
    public TextMeshProUGUI level3CompleteText;

    [Header("Newspaper Prefab")]
    public GameObject newspaperPrefab;

    public GameObject newspaperPosition;

    [Header("Other Stuff")]
    public int currentLevel;

    [HideInInspector]
    public List<CatScript> cats;

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
                UpdateCameraPosition(); // Add this line
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
        UpdateCameraPosition(); // Add this line
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
                        level1CompleteText.text = "Complete!";
                        level1CompleteText.color = Color.green;
                        break;
                    case 2:
                        level2CompleteText.text = "Complete!";
                        level2CompleteText.color = Color.green;
                        break;
                    case 3:
                        level3CompleteText.text = "Complete!";
                        level3CompleteText.color = Color.green;
                        break;
                }
            }
            else
            {
                switch (currentLevel)
                {
                    case 1:
                        level1CompleteText.text = "Not Completed";
                        level1CompleteText.color = Color.red;
                        break;
                    case 2:
                        level2CompleteText.text = "Not Completed";
                        level2CompleteText.color = Color.red;
                        break;
                    case 3:
                        level3CompleteText.text = "Not Completed";
                        level3CompleteText.color = Color.red;
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
            if (currentLevel >= 1 && currentLevel < 3)
            {
                yield return StartCoroutine(Fade(0, 1));
            }

            currentLevel++;

            if (currentLevel >= LevelPositions.Length)
            {
                Debug.LogWarning("No more levels available");

                SetRandomImagesToNewspaper();
            }
            else
            {
                Debug.Log("Level " + currentLevel + " activated");

                LevelAssets.transform.parent = LevelPositions[currentLevel].transform;
                LevelAssets.transform.localPosition = Vector3.zero;
                LevelAssets.transform.localRotation = Quaternion.identity;
                UpdateCameraPosition(); // Add this line
            }

            SoundManager.Instance.PlayButtonClick();

            if (currentLevel > 1 && currentLevel <= 3)
            {
                yield return StartCoroutine(Fade(1, 0));
            }
        }
    }

    public void SetRandomImagesToNewspaper()
    {
        List<Texture2D> allPictures = new List<Texture2D>();
        foreach (CatScript cat in cats)
        {
            if (cat.quest != null && cat.quest.questPhoto != null)
            {
                allPictures.Add(cat.quest.questPhoto);
            }
        }

        Texture2D[] randomPictures = new Texture2D[3];

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, allPictures.Count);

            randomPictures[i] = allPictures[randomIndex];
            allPictures.RemoveAt(randomIndex);
        }

        GameObject newspaperSpawned = Instantiate(newspaperPrefab, newspaperPosition.transform.position,
            newspaperPosition.transform.rotation);
        newspaperSpawned.GetComponent<Newspaper>().SetImage(randomPictures[0], randomPictures[1], randomPictures[2]);
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

    private void UpdateCameraPosition() // Add this method
    {
        if (currentLevel < CameraSpawnPositions.Length)
        {
            PhotoCamera photoCamera = FindObjectOfType<PhotoCamera>();
            if (photoCamera != null)
            {
                photoCamera.transform.position = CameraSpawnPositions[currentLevel].position;
                photoCamera.transform.rotation = CameraSpawnPositions[currentLevel].rotation;
            }
            else
            {
                Debug.LogWarning("PhotoCamera not found in the scene.");
            }
        }
        else
        {
            Debug.LogWarning("No camera spawn position defined for this level.");
        }
    }
}
