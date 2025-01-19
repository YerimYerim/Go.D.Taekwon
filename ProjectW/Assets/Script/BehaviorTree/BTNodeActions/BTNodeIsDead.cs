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

            CommandManager.Instance.AddCommand(new EnemyTurnCommand(() =>
            {
                children[0].Evaluate();
            }), 0.1f);
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
