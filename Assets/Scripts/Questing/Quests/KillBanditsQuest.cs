using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBanditsQuest : QuestNew
{
    private void Awake()
    {
        identifier = 1;
        QuestName = "Kill Bandits";
        Description = "Defeat bandits that are harrassing the village.";
        ExperienceReward = 50;

        Goals.Add(new KillGoal(this, 0, "Kill 5 bandits", false, 0, 5, Goal.type.kill));

        Goals.ForEach(g => g.Init());
    }
}
