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
            {
                var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
                if (gameBattleMode == null)
                {
                    return;
                }
                gameBattleMode.BattleHandler.AddSource(data.content_id ?? 0);
            } break;
            case REWARD_TYPE.REWARD_TYPE_SUPPORT_MODULE:
                GameSupportModuleManager.Instance.AddModule(data.content_id ?? 0);
                break;
            case REWARD_TYPE.REWARD_TYPE_MONEY:
                break;
            case REWARD_TYPE.REWARD_TYPE_SPELL_COMBINE:
            {
                var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
                if (gameBattleMode == null)
                {
                    return;
                }
                gameBattleMode.BattleHandler.AddSpellCombine(data.content_id ?? 0);
            }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(data.reward_type), data.content_id ?? 0, null);
        }
    }

    public void ShowRewardSelect(List<RewardTableData> data)
    {
        if (GameUIManager.Instance.TryGetOrCreate<UI_PopUp_RewardSelect>(false, UILayer.LEVEL_4, out var ui))
        {
            ui.Show();
            ui.SetData(data);
        }
    }

    public List<RewardTableData> SetSelectRewards()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        MapData map = battleMode.MapHandler.GetCurMap();

        var mapTable = GameTableManager.Instance._contentMapTableDatas.Find(_ => _.map_id == map.mapId);
        List<RewardTableData> rewardTableDatas = new List<RewardTableData>();
        if (mapTable != null)
        {
            int[] selectReward = mapTable.select_reward_id;

            if (selectReward.Length > 0)
            {
                foreach (var rewardId in selectReward)
                {
                    var rewardTable = GameTableManager.Instance._rewardTable.FindAll(_ => _.reward_id == rewardId);
                    for(int i = 0; i< rewardTable.Count; ++i)
                    {
                        if (IsExceptionReward(rewardTable[i]))
                        {
                            rewardTableDatas.Add(rewardTable[i]);
                        }
                    }
                }
            }
        }

        return rewardTableDatas;
    }

    public static bool IsExceptionReward(RewardTableData reward)
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (battleMode == null)
        {
            return false;
        }

        if (reward.exception_type == null)
        {
            return true;
        }

        var exceptionValue = reward.exception_value;
        switch (reward.exception_type)
        {
            case EXCEPTION_TYPE.EXCEPTION_TYPE_OWN_SPELL_SOURCE:
                for (int i = 0; i < exceptionValue.Length; ++i)
                {
                    if (battleMode.BattleHandler._sources.Exists(_ => _.GetSourceId() == exceptionValue[i]))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            case EXCEPTION_TYPE.EXCEPTION_TYPE_NO_SPELL_SOURCE:
                for (int i = 0; i < exceptionValue.Length; ++i)
                {
                    if (battleMode.BattleHandler._sources.Exists(_ => _.GetSourceId() == exceptionValue[i]))
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }
                return true;
            case EXCEPTION_TYPE.EXCEPTION_TYPE_CLEAR_MAP:
                for(int i = 0; i< exceptionValue.Length;++i)
                {
                    if (battleMode.MapHandler.IsClearMap(exceptionValue[i]))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
                break;
            case EXCEPTION_TYPE.EXCEPTION_TYPE_NOT_CLEAR_MAP:
                for (int i = 0; i < exceptionValue.Length; ++i)
                {
                    if (battleMode.MapHandler.IsClearMap(exceptionValue[i]))
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }
                break;

        }
        return true;
    }
}
