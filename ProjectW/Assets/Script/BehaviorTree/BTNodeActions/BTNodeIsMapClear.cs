using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class BTNodeIsMapClear : BTNodeAction
{
    public override State Evaluate()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (battleMode != null && battleMode.ActorHandler.IsAllEnemyDead())
        {
            var rewards = GameRewardManager.Instance.SetSelectRewards();
            GameRewardManager.Instance.ShowRewardSelect(rewards);
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
