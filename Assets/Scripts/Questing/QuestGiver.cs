using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    public QuestNew npcQuest;

    //If NPC can give quest we use these
    public Dialogue midQuestDialogue;
    public Dialogue postQuestDialogue;

    QuestHolder holder;

    //Different quest states
    bool hasGiven = false;
    bool hasCompleted = false;

    private void Awake()
    {
        holder = FindObjectOfType<QuestHolder>();
    }

    public override void TriggerDialogue()
    {
        if (!hasGiven && !hasCompleted)
        {
            base.TriggerDialogue();
        }
        else if (!hasCompleted)
        {
            StartDialogue(midQuestDialogue);
        }
        else
        {
            StartDialogue(postQuestDialogue);
        }
    }

    public override void EndDialogue()
    {
        if (!hasGiven)
        {
            holder.AddQuest(npcQuest);
            hasGiven = true;
            Debug.Log("Added quest from NPC");
        }
        else if (!hasCompleted && holder.FindQuest(npcQuest).Completed)
        {
            hasCompleted = true;
            npcQuest.GiveReward();
            Debug.Log("Quest completed. Turning in");
        }
        
        base.EndDialogue();
    }
}
