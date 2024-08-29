using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class BTNodeIsMapClear : BTNodeAction
{
    public override State Evaluate()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (battleMode != null && battleMode.ActorSpawner.IsAllEnemyDead())
        {
            Debug.Log("MapClear");
            GameTurnManager.Instance.AddTurnStack(GameTurnManager.TurnState.MapClear);
            battleMode?.MapHandler?.ShowMapSelect();
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
