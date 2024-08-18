using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsDraw : BTNodeAction
{
    public override State Evaluate()
    {
        if (GameBattleManager.Instance.IsDraw())
        {
//            Debug.Log("원소 카드를 뽑았습니다.");
            GameTurnManager.Instance.AddTurnStack(GameTurnManager.TurnState.Draw);
            return State.Success;
        };
        return State.Failure;
    }
}
