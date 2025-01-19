using System;
using System.Collections.Generic;
using Script.UI;
using TMPro;
using UnityEngine;

public class UI_PopUp_RewardResult : UIBase
{

    [SerializeField] private UIReward objReward;
    [SerializeField] private List<UIReward> objRewards = new List<UIReward>();
    [SerializeField] private GameObject objRewardParents;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI textDesc;
    
    [SerializeField] private DTButton closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
        SetHideEvent(HideAction);
    }

    private static void HideAction()
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        if (gameBattleMode == null)
        {
            return;
        }
        gameBattleMode?.ActorHandler?.RemoveAllMonsterActors();
        gameBattleMode?.MapHandler?.ShowMapSelect();
    }

    public void ShowRewardResult(List<RewardTableData> data)
    {
        Show();
        SetReward(data);
        for (int i = 0; i < data.Count; ++i)
        {
            GameRewardManager.Instance.GetReward(data[i]);
        }
    }

    private void SetReward(List<RewardTableData> data)
    {
        for (int i = 0; i < data.Count; ++i)
        {
            var index = i;

            if (objRewards.Count <= i)
            {
                var source = Instantiate(objReward.gameObject, objRewardParents.transform);
                objRewards.Add(source.GetComponent<UIReward>());
            }

            objRewards[index].gameObject.SetActive(true);
            objRewards[index].SetReward(data[index].reward_type ?? new REWARD_TYPE(), data[index].content_id ?? 0);
        }

        objReward.gameObject.SetActive(false);
    }
}
