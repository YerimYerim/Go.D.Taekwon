using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsEnemyTurn : BTNodeAction
{
    public override State Evaluate()
    {
        if (GameBattleManager.Instance.IsEnemyTurn())
        {
            Debug.Log("적의 턴");
            GameTurnManager.Instance.IsMyTurn = false;
            GameBattleManager.Instance.DoSKillEnemyTurn();
            return State.Success;
        };
        return State.Failure;
    }
}
