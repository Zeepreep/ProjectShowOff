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
        if (transform.childCount >= 1)
        {
            if (transform.GetComponentInChildren<Canvas>() != null)
            {
                canvas = transform.GetComponentInChildren<Canvas>();
                questNameText = canvas.transform.Find("Quest Name").GetComponent<TextMeshProUGUI>();
                questHintText = canvas.transform.Find("Quest Hint").GetComponent<TextMeshProUGUI>();
                questCompletedText = canvas.transform.Find("Quest Complete Status").GetComponent<TextMeshProUGUI>();
            }
            else
            {
                if (canvas == null)
                {
                    canvas = new GameObject().AddComponent<Canvas>();
                    canvas.name = "Canvas";
                    canvas.transform.SetParent(transform);
                    canvas.renderMode = RenderMode.WorldSpace;
                    canvas.transform.localPosition = new Vector3(0, 0, 0);
                    canvas.transform.localRotation = Quaternion.identity;

                    questNameText = InstantiateText("Quest Name", 15, new Vector3(0, 0.75f, 0));
                    questHintText = InstantiateText("Quest Hint", 12, new Vector3(0, 0.45f, 0));
                    questCompletedText = InstantiateText("Quest Complete Status", 12, new Vector3(0, -0.41f, 0));
                }
            }

            SetQuestText();
        }
        
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
        textObject.rectTransform.sizeDelta = new Vector2(80, 50);
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
        if (questNameText != null)
        {
            questNameText.text = name;
        }
        else
        {
            Debug.LogError("questNameText is null.");
        }
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

    public bool HasStringTo(PhotoSpot other)
    {
        foreach (Transform child in transform)
        {
            StringScript stringScript = child.GetComponent<StringScript>();
            if (stringScript != null && (stringScript.end1 == other || stringScript.end2 == other))
            {
                return true;
            }
        }

        foreach (Transform child in other.transform)
        {
            StringScript stringScript = child.GetComponent<StringScript>();
            if (stringScript != null && (stringScript.end1 == this || stringScript.end2 == this))
            {
                return true;
            }
        }

        return false;
    }
}