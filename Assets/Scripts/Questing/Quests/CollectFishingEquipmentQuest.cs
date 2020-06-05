using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFishingEquipmentQuest : QuestNew
{
    private void Awake()
    {
        identifier = 0;
        QuestName = "Collect Fishing Equipment";
        Description = "Get the fishing equipment and return it.";
        ExperienceReward = 50;

        Goals.Add(new CollectGoal(this, 0, "Find the fishing rod.", false, 0, 0, Goal.type.collect));

        Goals.ForEach(g => g.Init());
    }
}
