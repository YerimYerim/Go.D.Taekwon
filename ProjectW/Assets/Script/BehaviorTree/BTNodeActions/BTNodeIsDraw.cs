using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsDraw : BTNodeAction
{
    public override State Evaluate()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (battleMode != null && battleMode.BattleHandler.IsDraw())
        {
            GameTurnManager.Instance.AddTurnStack(GameTurnManager.TurnState.Draw);
            return State.Success;
        };
        return State.Failure;
    }
}
