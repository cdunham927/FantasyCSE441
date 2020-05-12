using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [Header("Quest information")]
    public int questIdentifier;
    public string questName;
    public string questDescription;

    public enum QuestTypes { fetch, kill, objective }
    public QuestTypes questType = QuestTypes.fetch;

    [Header("Quest rewards")]
    public float goldReward;
    public float expReward;

    [HideInInspector]
    public bool isCompleted = false;

    [Header("Completion status")]
    [Range(0, 4)]
    public int objectives = 0;
    public string[] objectiveText;
    int objectivesCompleted = 0;
    [Range(0, 10)]
    public int enemiesToKill = 0;
    int curEnemyKills = 0;
    [Range(0, 10)]
    public int itemsToFetch = 0;
    int curItemsFetched = 0;

    public void CompleteQuest()
    {
        if (!isCompleted)
        {
            //FindObjectOfType<PlayerController>().AddGold(goldReward);
            //FindObjectOfType<PlayerController>().AddExp(expReward);
            Debug.Log("Completed " + questName + " !");
            isCompleted = true;
        }
        else Debug.Log("Quest already completed!");
    }

    public void UpdateQuest()
    {
        switch(questType)
        {
            case (QuestTypes.fetch):
                curItemsFetched++;
                Debug.Log("Updated Fetch Quest");
                if (curItemsFetched >= itemsToFetch) CompleteQuest();
                break;
            case (QuestTypes.kill):
                curEnemyKills++;
                Debug.Log("Updated Kill Quest");
                if (curEnemyKills >= enemiesToKill) CompleteQuest();
                break;
            case (QuestTypes.objective):
                objectivesCompleted++;
                Debug.Log("Updated Objective Quest");
                if (objectivesCompleted >= objectives) CompleteQuest();
                break;
        }
    }

    public void AcceptQuest()
    {
        FindObjectOfType<QuestHolder>().AddQuest(this);
    }
}
