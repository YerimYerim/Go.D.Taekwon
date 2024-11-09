using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class UI_PopUp_RewardSelect : UIBase
{
    [SerializeField] private UIPopUpRewardSelectObject[] _rewardSelectObjects;
    

    public void SetData(List<RewardTableData> data)
    {
        for(int i = 0; i< _rewardSelectObjects.Length; ++i)
        {
            if (i < data.Count)
            {
                _rewardSelectObjects[i].SetData(data[i]?.reward_type?? new REWARD_TYPE(), data[i]?.content_id ??0);
                var captureIndex = i;
                _rewardSelectObjects[i].SetButtonEvent(()=>OnClickSelectReward(data[captureIndex]));
                _rewardSelectObjects[i].gameObject.SetActive(true);
            }
            else
            {
                _rewardSelectObjects[i].gameObject.SetActive(false);
            }
        }
    }
    public void OnClickSelectReward(RewardTableData data)
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (gameBattleMode == null)
        {
            return;
        }
        if (GameUIManager.Instance.TryGetOrCreate<UI_PopUp_RewardResult>(true, UILayer.LEVEL_3, out var rewardResult))
        {
            var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
            if (battleMode == null)
            {
                return;
            }

            var rewardTableDatas = battleMode.MapHandler.GetRewardTableDatas();
            rewardTableDatas.Add(data);
            rewardResult.ShowRewardResult(rewardTableDatas);
        }
        Hide();
    }
}