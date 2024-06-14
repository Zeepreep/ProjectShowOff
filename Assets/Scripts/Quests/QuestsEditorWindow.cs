using System.Collections;
using System.Collections.Generic;
using Management;
using TMPro;
using UnityEditor;
using UnityEngine;

public class QuestsEditorWindow : EditorWindow
{
    private static Quests quests;
    private QuestApplier _questApplier;

    public GameObject stringPrefab;

    [MenuItem("Window/Quests Editor")]
    public static void ShowWindow()
    {
        GetWindow<QuestsEditorWindow>("Quests Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Quest Settings", EditorStyles.boldLabel);

        // Display the game settings name
        if (quests != null)
        {
            GUILayout.Space(7.5f);
            GUILayout.Label($"Editing: {quests.name}");
        }

        GUILayout.Space(15);

        // Buttons for creating, loading, and saving game settings
        GUILayout.BeginHorizontal();

        // Button to create a new game settings
        if (GUILayout.Button("Create Quests"))
        {
            CreateQuests();
        }

        // Button to load game settings
        if (GUILayout.Button("Load Quests"))
        {
            LoadQuests();
        }

        // Button to save game settings
        if (quests != null && GUILayout.Button("Save Quests"))
        {
            SaveQuests();
        }

        // Button to apply game settings
        if (quests != null && GUILayout.Button("Apply Quests"))
        {
            SaveQuests();
            ApplyQuests();
            // Implement logic to apply game settings
        }

        GUILayout.EndHorizontal();

        if (quests == null)
        {
            EditorGUILayout.HelpBox("Create or load quests to begin.", MessageType.Info);
            return;
        }

        // Display parameters to edit
        DisplayParameters();
    }

    private void CreateQuests()
    {
        string path =
            EditorUtility.SaveFilePanel("Create Quests", "Assets/Quests", "NewQuests", "asset");

        if (path.Length != 0)
        {
            string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
            quests = ScriptableObject.CreateInstance<Quests>();
            AssetDatabase.CreateAsset(quests, relativePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private void LoadQuests()
    {
        string path = EditorUtility.OpenFilePanel("Load Quests", "Assets/Quests", "asset");

        if (path.Length != 0)
        {
            string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
            quests = AssetDatabase.LoadAssetAtPath<Quests>(relativePath);
        }
    }

    private void SaveQuests()
    {
        EditorUtility.SetDirty(quests);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void ApplyQuests()
    {
        if (_questApplier == null)
        {
            _questApplier = QuestApplier.Instance;
        }

        if (_questApplier == null)
        {
            _questApplier = FindObjectOfType<QuestApplier>();
        }

        if (_questApplier != null)
        {
            _questApplier.ApplySettings(quests);

            if (quests != null)
            {
                CreateStrings();
            }
        }

        else

        {
            Debug.LogWarning("No QuestApplier");
        }
    }

    private Vector2 scrollPosition;


    private void DisplayParameters()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.Space(15);

        EditorGUILayout.LabelField("Editor Inputs", EditorStyles.boldLabel);

        stringPrefab =
            EditorGUILayout.ObjectField("String Prefab", stringPrefab, typeof(GameObject), true) as GameObject;

        GUILayout.Space(10);

        // Create a header to display quest settings
        EditorGUILayout.LabelField("Quest Settings", EditorStyles.boldLabel);

        GUILayout.Space(10);

        for (int i = 0; i < quests.quests.Count; i++)
        {
            Quest quest = quests.quests[i];

            EditorGUILayout.LabelField($"Quest {i + 1}", EditorStyles.boldLabel);

            quest.questName = EditorGUILayout.TextField("Quest Name", quest.questName);
            quest.questHint = EditorGUILayout.TextField("Quest Hint", quest.questHint);

            quest.questPhotoSpot =
                (PhotoSpot)EditorGUILayout.ObjectField("Quest Photo Spot", quest.questPhotoSpot,
                    typeof(PhotoSpot), true);

            quest.questCat =
                (CatScript)EditorGUILayout.ObjectField("Quest Cat", quest.questCat, typeof(CatScript), true);
            quest.correspondingLevel = EditorGUILayout.IntField("Corresponding Level", quest.correspondingLevel);

            GUILayout.Space(10);

            EditorGUILayout.LabelField("Quest Debug Settings", EditorStyles.boldLabel);

            quest.isActive = EditorGUILayout.Toggle("Is Active", quest.isActive);
            quest.isCompleted = EditorGUILayout.Toggle("Is Completed", quest.isCompleted);
            
            GUILayout.Space(10);
        }

        if (GUILayout.Button("Add Quest"))
        {
            quests.quests.Add(new Quest());
        }

        if (quests.quests.Count > 0 && GUILayout.Button("Remove Last Quest"))
        {
            quests.quests.RemoveAt(quests.quests.Count - 1);
        }

        EditorGUILayout.EndScrollView();
    }

    private void CreateStrings()
    {
        if (stringPrefab == null)
        {
            Debug.LogError("String prefab is not assigned");
            return;
        }

        for (int i = 0; i < quests.quests.Count - 1; i++)
        {
            Quest quest1 = quests.quests[i];
            Quest quest2 = quests.quests[i + 1];

            if (quest1.questPhotoSpot != null && quest2.questPhotoSpot != null)
            {
                if (!quest1.questPhotoSpot.HasStringTo(quest2.questPhotoSpot) &&
                    !quest2.questPhotoSpot.HasStringTo(quest1.questPhotoSpot))
                {
                    GameObject newString = Instantiate(stringPrefab);

                    if (newString == null)
                    {
                        Debug.LogError("Failed to instantiate string prefab");
                        continue;
                    }

                    StringScript stringScript = newString.GetComponent<StringScript>();

                    if (stringScript == null)
                    {
                        Debug.LogError("String prefab does not have a StringScript component");
                        continue;
                    }

                    Vector3 position = (quest1.questPhotoSpot.transform.position +
                                        quest2.questPhotoSpot.transform.position) / 2;

                    Vector3 direction = quest1.questPhotoSpot.transform.position -
                                        quest2.questPhotoSpot.transform.position;

                    Quaternion rotationOffset = Quaternion.Euler(-90, 0, 0);

                    Quaternion rotation = Quaternion.LookRotation(direction) * rotationOffset;

                    float distance = Vector3.Distance(quest1.questPhotoSpot.transform.position,
                        quest2.questPhotoSpot.transform.position);
                    Vector3 scale = new Vector3(0.1f, distance / 2,
                        0.1f);

                    newString.transform.position = position;
                    newString.transform.rotation = rotation;
                    newString.transform.localScale = scale;

                    // Parent the new string to the PhotoSpot objects
                    newString.transform.parent = quest1.questPhotoSpot.transform;

                    // Set the ends of the string
                    stringScript.SetEnds(quest1.questPhotoSpot, quest2.questPhotoSpot);
                }
                else
                {
                    Debug.Log("Update Strings");
                    UpdateStrings();
                }
            }
        }
    }
    
    private void UpdateStrings()
    {
        for (int i = 0; i < quests.quests.Count - 1; i++)
        {
            Quest quest1 = quests.quests[i];
            Quest quest2 = quests.quests[i + 1];

            if (quest1.questPhotoSpot != null && quest2.questPhotoSpot != null)
            {
                foreach (Transform child in quest1.questPhotoSpot.transform)
                {
                    StringScript stringScript = child.GetComponent<StringScript>();
                    if (stringScript != null && (stringScript.end1 == quest2.questPhotoSpot || stringScript.end2 == quest2.questPhotoSpot))
                    {
                        stringScript.UpdateString();
                    }
                }
            }
        }
    }

}