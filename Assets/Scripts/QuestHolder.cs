﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHolder : MonoBehaviour
{
    public List<QuestNew> quests = new List<QuestNew>();
    public List<QuestNew> finishedQuests = new List<QuestNew>();

    public void AddQuest(QuestNew quest)
    {
        if (!quests.Contains(quest))
        {
            QuestNew q = gameObject.AddComponent(System.Type.GetType(quest.name)) as QuestNew;
            //Debug.Log("Added " + quest.QuestName + " to active quests!");
            quests.Add(q);
        }
        //else Debug.Log("Already have this quest");
    }

    public bool FindFinishedQuest(int id)
    {
        foreach (QuestNew q in finishedQuests)
        {
            if (q.identifier == id) return true;
        }
        return false;
    }

    public bool HasFinishedQuest(QuestNew quest)
    {
        return finishedQuests.Contains(quest);
    }

    public void Remove(QuestNew quest)
    {
        QuestNew q = FindQuest(quest.identifier);
        finishedQuests.Add(q);
        quests.Remove(q);
    }

    public bool HasQuest(int id)
    {
        foreach (QuestNew q in quests)
        {
            if (q.identifier == id)
            {
                //Debug.Log(q.QuestName + ": " + q.identifier);
                return true;
            }
        }
        //Debug.Log("Quest with id " + id + " not found");
        return false;
    }

    public QuestNew FindQuest(int identifier)
    {
        foreach (QuestNew q in quests)
        {
            if (q.identifier == identifier) return q;
        }
        return null;
    }

    public QuestNew FindQuest(QuestNew quest)
    {
        foreach (QuestNew q in quests)
        {
            if (q == quest) return q;
        }
        return null;
    }

    public QuestNew FindQuest(string questName)
    {
        foreach(QuestNew q in quests)
        {
            if (q.QuestName == questName) return q;
        }
        return null;
    }

    public void UpdateKillQuests(int id)
    {
        foreach (QuestNew q in quests)
        {
                foreach (Goal g in q.Goals)
                {
                    if (g.goalType == Goal.type.kill)
                    {
                        g.Collect(id);
                    }
                }
        }
    }

    public void UpdateCollectQuests(int id)
    {
        foreach (QuestNew q in quests)
        {
                foreach (Goal g in q.Goals)
                {
                    if (g.goalType == Goal.type.collect)
                    {
                        g.Collect(id);
                    }
                }
        }
    }

    //For testing
    public void TestKill()
    {
        QuestNew q = FindQuest("Kill Bandits");
        if (q != null) q.CheckGoals();
        else Debug.Log("Don't have this quest!");
    }

    public void TestFetch()
    {
        QuestNew q = FindQuest("Find Ring");
        if (q != null) q.CheckGoals();
        else Debug.Log("Don't have this quest!");
    }

    public void TestObjective()
    {
        QuestNew q = FindQuest("ETC Objective");
        if (q != null) q.CheckGoals();
        else Debug.Log("Don't have this quest!");
    }
}
