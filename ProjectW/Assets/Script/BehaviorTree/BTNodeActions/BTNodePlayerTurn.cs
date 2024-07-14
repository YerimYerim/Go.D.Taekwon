using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodePlayerTurn : BTNodeAction
{
    public override State Evaluate()
    {
        Debug.Log("공격 가능!");
        GameTurnManager.Instance.IsMyTurn = true;
        return State.Running;
    }
}
