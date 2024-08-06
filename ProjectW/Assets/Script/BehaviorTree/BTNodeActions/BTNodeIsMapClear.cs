using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsMapClear : BTNodeAction
{
    public override State Evaluate()
    {
        if (GameBattleManager.Instance.IsAllEnemyDead())
        {
            Debug.Log("MapClear");
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
