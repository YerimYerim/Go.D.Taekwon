using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsEnemyTurn : BTNodeAction
{
    public override State Evaluate()
    {
        if (BattleManager.Instance.IsEnemyTurn())
        {
            Debug.Log("적의 턴");
            GameTurnManager.Instance.isMyTurn = false;
            return State.Success;
        };
        return State.Failure;
    }
}
