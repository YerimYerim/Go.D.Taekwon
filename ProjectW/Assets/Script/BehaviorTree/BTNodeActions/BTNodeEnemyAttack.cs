using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeEnemyAttack : BTNodeAction
{
    public override State Evaluate()
    {
        Debug.Log("BTNodeEnemyAttack");
        if (GameBattleManager.Instance.IsEnemyTurn())
        {
            return State.Running;
        };
        return State.Failure;
    }
}
