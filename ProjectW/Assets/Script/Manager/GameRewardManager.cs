using System;
using System.Collections.Generic;
using Script.Manager;

public class GameRewardManager : Singleton<GameRewardManager>
{
    //reward id , reward
    Dictionary<int?, List<RewardTableData>> _rewardTable = new();
    
    
    public void Init()
    {
        var rewardTable = GameTableManager.Instance._rewardTable;
        foreach (var reward in rewardTable)
        {
            if (reward.reward_id != null && !_rewardTable.ContainsKey(reward.reward_id))
            {
                _rewardTable.Add(reward.reward_id, new List<RewardTableData>());
            }

            if (reward.reward_id != null)
            {
                _rewardTable[reward.reward_id].Add(reward);
            }
        }
    }
    
    /// <summary>
    /// 득템시 
    /// </summary>
    /// <param name="rewardType"></param>
    /// <param name="rewardID"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void GetReward(RewardTableData data)
    {
        switch (data.reward_type)
        {
            case REWARD_TYPE.REWARD_TYPE_ITEM:
                break;
            case REWARD_TYPE.REWARD_TYPE_SPELL_SOURCE:
                var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
                if (gameBattleMode == null)
                {
                    return;
                }
                gameBattleMode.BattleHandler.AddSource(data.content_id ?? 0);
                break;
            case REWARD_TYPE.REWARD_TYPE_SUPPORT_MODULE:
                GameSupportModuleManager.Instance.AddModule(data.content_id ?? 0);
                break;
            case REWARD_TYPE.REWARD_TYPE_MONEY:
                break;  
            default:
                throw new ArgumentOutOfRangeException(nameof(data.reward_type), data.content_id ?? 0, null);
        }
    }

    private int GetRandomCount(RewardTableData data)
    {
        return UnityEngine.Random.Range(data.cnt_min ??0, data.cnt_max??0 + 1);
    }
    public void ShowRewardSelect(List<RewardTableData> data)
    {
        if (GameUIManager.Instance.TryGetOrCreate<UI_PopUp_RewardSelect>(false, UILayer.LEVEL_4, out var ui ))
        {
            ui.Show();
            ui.SetData(data);
        }
    }
    
}
