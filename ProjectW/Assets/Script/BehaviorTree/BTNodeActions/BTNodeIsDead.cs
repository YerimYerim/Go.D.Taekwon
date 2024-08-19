using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsDead : BTNodeAction
{
    
    public override State Evaluate()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        
        if (battleMode!= null && battleMode.BattleHandler.GetMyHp() <= 0)
        {
  //          Debug.Log("PlayerDead");
            return State.Success;
        }
        else
        {
//            Debug.Log("PlayerNotDead");
            return State.Failure;
        }
    }
}
