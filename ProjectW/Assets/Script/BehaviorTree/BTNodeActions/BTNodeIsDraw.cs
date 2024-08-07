using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsDraw : BTNodeAction
{
    public override State Evaluate()
    {
        if (GameBattleManager.Instance.IsDraw())
        {
            Debug.Log("원소 카드를 뽑았습니다.");
            GameBattleManager.Instance.passivePoint += 1;
            return State.Success;
        };
        return State.Failure;
    }
}
