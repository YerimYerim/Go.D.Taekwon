using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNodeIsEnemyTurn : BTNodeAction
{
    public override State Evaluate()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (battleMode !=null && battleMode.ActorSpawner.IsEnemyTurn())
        {
            Debug.Log("적의 턴");
            GameTurnManager.Instance.AddTurnStack(GameTurnManager.TurnState.EnemyTurn);
            battleMode.BattleHandler.DoSkillEnemyTurn();
            return State.Success;
        };
        return State.Failure;
    }
}
