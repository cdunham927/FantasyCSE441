using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHolder : MonoBehaviour
{
    public List<QuestNew> quests = new List<QuestNew>();

    public void AddQuest(QuestNew quest)
    {
        if (!quests.Contains(quest))
        {
            Debug.Log("Added " + quest.QuestName + " to active quests!");
            quests.Add(quest);
        }
        else Debug.Log("Already have this quest");
    }

    public void Remove(QuestNew quest)
    {
        quests.Remove(quest);
    }

    public QuestNew FindQuest(int identifier)
    {
        foreach (QuestNew q in quests)
        {
            if (q.identifier == identifier && !q.Completed) return q;
        }
        return null;
    }

    public QuestNew FindQuest(QuestNew quest)
    {
        foreach (QuestNew q in quests)
        {
            if (q == quest && !q.Completed) return q;
        }
        return null;
    }

    public QuestNew FindQuest(string questName)
    {
        foreach(QuestNew q in quests)
        {
            if (q.QuestName == questName && !q.Completed) return q;
        }
        return null;
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
