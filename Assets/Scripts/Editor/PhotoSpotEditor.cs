using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PhotoSpot))]
public class PhotoSpotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PhotoSpot photoSpot = (PhotoSpot)target;

        DrawDefaultInspector();

        PhotoSpot[] photoSpots = GameObject.FindObjectsOfType<PhotoSpot>();

        List<string> options = new List<string>();
        List<Quest> quests = new List<Quest>();
        for (int i = 0; i < photoSpots.Length; i++)
        {
            if (photoSpots[i].quest != null)
            {
                options.Add(photoSpots[i].quest.questName);
                quests.Add(photoSpots[i].quest);
            }
        }

        int selectedIndex = quests.IndexOf(photoSpot.quest);

        selectedIndex = EditorGUILayout.Popup("Quest", selectedIndex, options.ToArray());

        if (selectedIndex != -1)
        {
            photoSpot.quest = quests[selectedIndex];
        }
    }
}