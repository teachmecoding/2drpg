﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInventory : MonoBehaviour
{
    public List<Quest> activeQuests = new List<Quest>();
    QuestDatabase questDatabase;
    UIQuestHandler uiQuestHandler;

    private void Start()
    {
        questDatabase = FindObjectOfType<QuestDatabase>();
    }

    public void ReceiveQuest(Quest questToAdd)
    {
        uiQuestHandler = FindObjectOfType<UIQuestHandler>();
        Quest questObject = questDatabase.GetQuest(questToAdd.id);

        bool alreadyHave = CheckActiveQuest(questObject);
        if (questObject != null && questObject.status == Quest.Quest_status.Waiting && CheckActiveQuest(questObject) == false)
        {
            activeQuests.Add(questObject);
            uiQuestHandler.UpdateQuestButton(questToAdd);
            questObject.status = Quest.Quest_status.InProgress;
        }
    }

    public void ReceiveQuest(int questIdToAdd)
    {
        uiQuestHandler = FindObjectOfType<UIQuestHandler>();
        Quest questToAdd = questDatabase.GetQuest(questIdToAdd);
        if (questToAdd != null && questToAdd.status == Quest.Quest_status.Waiting && CheckActiveQuest(questToAdd) == false)
        {
            activeQuests.Add(questToAdd);
            uiQuestHandler.UpdateQuestButton(questToAdd);
            questToAdd.status = Quest.Quest_status.InProgress;
        }
    }

    public void RemoveQuest(Quest questToRemove)
    {
        uiQuestHandler = FindObjectOfType<UIQuestHandler>();

        if (questToRemove != null && CheckActiveQuest(questToRemove) == true)
        {

            uiQuestHandler.ResetQuestButton(questToRemove);

            int slot = FindIndexOfQuest(questToRemove);
            activeQuests.RemoveAt(slot);
            
            if (questToRemove.status == Quest.Quest_status.InProgress || questToRemove.status == Quest.Quest_status.ReadyToDeliver)
            {
                questToRemove.status = Quest.Quest_status.Waiting;
            }
           
        }
    }

    public void RemoveQuest(int questIdToRemove)
    {
        uiQuestHandler = FindObjectOfType<UIQuestHandler>();
        Quest questToRemove = questDatabase.GetQuest(questIdToRemove);

        if (questToRemove != null && CheckActiveQuest(questIdToRemove) == true)
        {
            int slot = FindIndexOfQuest(questToRemove);
            activeQuests.RemoveAt(slot);
            uiQuestHandler.ResetQuestButton(questToRemove);
            if (questToRemove.status == Quest.Quest_status.InProgress || questToRemove.status == Quest.Quest_status.ReadyToDeliver)
            {
                questToRemove.status = Quest.Quest_status.Waiting;
            }
        }
    }

    int FindIndexOfQuest(Quest quest)
    {
        return activeQuests.FindIndex(item => item.id == quest.id);
    }

    public bool CheckActiveQuest(Quest questToCheck)
    {
        try
        {
            Quest quest = activeQuests.Find(questObject => questObject.id == questToCheck.id);
            if (quest != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public bool CheckActiveQuest(int questIdToCheck)
    {
        try
        {
            Quest quest = activeQuests.Find(questObject => questObject.id == questIdToCheck);
            if (quest != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch { return false; }
        
    }
}
