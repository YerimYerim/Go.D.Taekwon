using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsDead : BTNodeAction
{
    
    public override State Evaluate()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var handler = battleMode?.PlayerActorHandler;

        if (handler?.GetPlayerHp() <= 0)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
