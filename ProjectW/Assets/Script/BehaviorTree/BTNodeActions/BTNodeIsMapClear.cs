using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class BTNodeIsMapClear : BTNodeAction
{
    public override State Evaluate()
    {
        if (GameBattleManager.Instance.IsAllEnemyDead())
        {
            Debug.Log("MapClear");
            GameTurnManager.Instance.AddTurnStack(GameTurnManager.TurnState.MapClear);
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
