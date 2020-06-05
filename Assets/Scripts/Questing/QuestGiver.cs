using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    public QuestNew npcQuest;

    //If NPC can give quest we use these
    public Dialogue midQuestDialogue;
    public Dialogue finishQuestDialogue;
    public Dialogue postQuestDialogue;

    QuestHolder holder;

    //Different quest states
    [SerializeField]
    bool hasGiven = false;
    [SerializeField]
    bool hasCompleted = false;

    private void OnEnable()
    {
        Invoke("SetupQuest", 0.1f);
    }

    void SetupQuest()
    {
        holder = FindObjectOfType<QuestHolder>();
        hasGiven = holder.HasQuest(npcQuest.identifier);
        if (hasGiven) hasCompleted = holder.HasFinishedQuest(holder.FindQuest(npcQuest.identifier));
        if (holder.FindFinishedQuest(npcQuest.identifier))
        {
            hasGiven = true;
            hasCompleted = true;
        }
    }

    public override void TriggerDialogue()
    {
        if (!hasGiven && !hasCompleted)
        {
            base.TriggerDialogue();
        }
        else if ((holder.FindQuest(npcQuest.identifier) != null && !holder.FindQuest(npcQuest.identifier).Completed))
        {
            StartDialogue(midQuestDialogue);
        }
        else if (!hasCompleted)
        {
            StartDialogue(finishQuestDialogue);
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
            //Debug.Log("Added " + npcQuest.name + " from NPC");
        }
        else if (!hasCompleted && (holder.FindQuest(npcQuest.identifier) != null && holder.FindQuest(npcQuest.identifier).Completed))
        {
            hasCompleted = true;
            npcQuest.GiveReward();
            //Debug.Log("Quest completed. Turning in");
        }
        
        base.EndDialogue();
    }
}
