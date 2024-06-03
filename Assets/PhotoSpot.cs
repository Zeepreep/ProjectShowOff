using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PhotoSpot : MonoBehaviour
{
    public GameObject photoSphere;

    [HideInInspector] public TextMeshProUGUI questNameText;
    [HideInInspector] public TextMeshProUGUI questHintText;
    [HideInInspector] public TextMeshProUGUI questCompletedText;

    [HideInInspector] public Quest quest;
    
    private Canvas canvas;

    private void Awake()
    {
        if (photoSphere == null)
        {
            Debug.LogWarning("PhotoSphere is not assigned to the PhotoSpot");
        }
    }
    
    public void CreateTexts()
    {
        if (photoSphere.transform.childCount > 0)
        {
            SetQuestText();
            return;
        }
        
        canvas = photoSphere.GetComponentInChildren<Canvas>();
        if (canvas == null)
        {
            canvas = new GameObject().AddComponent<Canvas>();
            canvas.name = "Canvas";
            canvas.transform.SetParent(photoSphere.transform);
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.transform.localPosition = new Vector3(0, 0, 0);
            canvas.transform.localRotation = Quaternion.identity;
        }

        questNameText = InstantiateText("Quest Name", 8, new Vector3(0, 0.47f, 0));
        questHintText = InstantiateText("Quest Hint", 6, new Vector3(0, 0.3f, 0));
        questCompletedText = InstantiateText("Quest Complete Status", 6, new Vector3(0, -0.3f, 0));
        
        SetQuestText();
    }

    TextMeshProUGUI InstantiateText(string text, int fontSize, Vector3 position)
    {
        TextMeshProUGUI textObject = new GameObject().AddComponent<TextMeshProUGUI>();
        textObject.name = text;
        textObject.transform.SetParent(canvas.transform);
        textObject.transform.localPosition = position;
        textObject.transform.localRotation = Quaternion.identity;
        textObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        textObject.rectTransform.sizeDelta = new Vector2(70, 50);
        textObject.text = text;
        textObject.fontSize = fontSize;
        textObject.color = Color.white;
        textObject.alignment = TextAlignmentOptions.Center;
        return textObject;
    }
    
    public void SetQuestText()
    {
        SetQuestNameText(quest.questName);
        SetQuestHintText(quest.questHint);
        SetQuestCompletedText(quest.isCompleted);
    }

    public void SetQuestNameText(string name)
    {
        questNameText.text = name;
    }

    public void SetQuestHintText(string hint)
    {
        questHintText.text = hint;
    }

    public void SetQuestCompletedText(bool completed)
    {
        if (completed)
        {
            questCompletedText.text = "Completed";
        }
        else
        {
            questCompletedText.text = "Not Completed";
        }
    }
}