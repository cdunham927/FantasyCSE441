using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHolder : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();

    public void AddQuest(Quest quest)
    {
        if (!quests.Contains(quest))
        {
            Debug.Log("Added " + quest.questName + " to active quests!");
            quests.Add(quest);
        }
        else Debug.Log("Already have this quest");
    }

    public void Remove(Quest quest)
    {
        quests.Remove(quest);
    }

    public Quest FindQuest(int identifier)
    {
        foreach (Quest q in quests)
        {
            if (q.questIdentifier == identifier && !q.isCompleted) return q;
        }
        return null;
    }

    public Quest FindQuest(Quest quest)
    {
        foreach (Quest q in quests)
        {
            if (q == quest && !q.isCompleted) return q;
        }
        return null;
    }

    public Quest FindQuest(string questName)
    {
        foreach(Quest q in quests)
        {
            if (q.questName == questName && !q.isCompleted) return q;
        }
        return null;
    }

    //For testing
    public void TestKill()
    {
        Quest q = FindQuest("Kill Bandits");
        if (q != null) q.UpdateQuest();
        else Debug.Log("Don't have this quest!");
    }

    public void TestFetch()
    {
        Quest q = FindQuest("Find Ring");
        if (q != null) q.UpdateQuest();
        else Debug.Log("Don't have this quest!");
    }

    public void TestObjective()
    {
        Quest q = FindQuest("ETC Objective");
        if (q != null) q.UpdateQuest();
        else Debug.Log("Don't have this quest!");
    }
}
