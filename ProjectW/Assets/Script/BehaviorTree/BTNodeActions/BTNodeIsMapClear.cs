using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class BTNodeIsMapClear : BTNodeAction
{
    public override State Evaluate()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (battleMode != null && battleMode.ActorHandler.IsAllEnemyDead())
        {
            Debug.Log("MapClear");
            GameTurnManager.Instance.AddTurnStack(GameTurnManager.TurnState.MapClear);
            MapData map= battleMode.MapHandler.GetCurMap();

            var mapTable = GameTableManager.Instance._contentMapTableDatas.Find(_ => _.map_id == map.mapId);
            if (mapTable != null)
            {
                int[] selectReward = mapTable.select_reward_id;
                
                List<RewardTableData> rewardTableDatas = new List<RewardTableData>();
                if(selectReward.Length > 0)
                {
                    foreach (var rewardId in selectReward)
                    {
                        var rewardTable = GameTableManager.Instance._rewardTable.FindAll(_ => _.reward_id == rewardId);
                        rewardTableDatas.AddRange(rewardTable);
                    }
                }
                GameRewardManager.Instance.ShowRewardSelect(rewardTableDatas);
            }
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
