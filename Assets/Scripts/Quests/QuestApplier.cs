using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Management
{
    //debug line
    
    public class QuestApplier : MonoBehaviour
    {
        public static QuestApplier Instance;

        [Header("Default Game Settings Applied At Start")] [SerializeField]
        private Quests defaultQuests;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("More than one GameSettingsApplier in the scene");
                Destroy(this);
            }
        }

        void Start() 
        {
            ApplySettings(defaultQuests);
        }
    

        public void ApplySettings(Quests quests)
        {
            
            #region Quests Applied Settings
            
            Debug.Log("Applying Settings for Quests");
            
            foreach (Quest quest in quests.quests)
            {
                if (quest.questPhotoSpot != null)
                {
                    Debug.Log("Creating Quests Text");
                    
                    quest.questPhotoSpot.CreateTexts();
                    quest.questPhotoSpot.quest = quest;
                }
                else
                {
                    Debug.LogWarning(quest.questName + " does not have a PhotoSpot assigned");
                }
                
                if (quest.questCat != null)
                {
                    quest.questCat.quest = quest;
                }
                else
                {
                    Debug.LogWarning(quest.questName + " does not have a Cat assigned");
                }
            }
            
            #endregion
            
        }
    }
}