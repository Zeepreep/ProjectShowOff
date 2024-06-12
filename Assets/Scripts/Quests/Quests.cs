using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Quest
{
    public string questName;
    public string questHint;
    public CatScript questCat;
    public PhotoSpot questPhotoSpot;
    public int correspondingLevel;
    public bool isActive;
    public bool isCompleted;

    public List<GameObject> strings = new List<GameObject>();
}

[CreateAssetMenu(fileName = "Quests", menuName = "ScriptableObjects/Quests", order = 1)]
public class Quests : ScriptableObject
{
    public List<Quest> quests = new List<Quest>();
}