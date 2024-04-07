using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsDraw : BTNodeAction
{
    public override State Evaluate()
    {
        if (BattleManager.Instance.IsDraw())
        {
            Debug.Log("원소 카드를 뽑았습니다.");
            BattleManager.Instance.PassivePoint += 1;
            return State.Success;
        };
        return State.Failure;
    }
}
