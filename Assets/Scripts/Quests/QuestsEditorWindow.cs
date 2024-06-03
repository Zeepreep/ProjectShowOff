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
        }
        else
        {
            Debug.LogWarning("No QuestApplier");
        }
    }

    private Vector2 scrollPosition;
    private bool questSettingsFoldout = false;
    private bool quest1Foldout = false;
    private bool quest2Foldout = false;


    private void DisplayParameters()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.Space(15);

        // Create a header to display quest settings
        EditorGUILayout.LabelField("Quest Settings", EditorStyles.boldLabel);

        GUILayout.Space(15);

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
                (CatHandler)EditorGUILayout.ObjectField("Quest Cat", quest.questCat, typeof(CatHandler), true);
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
}