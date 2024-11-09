using System;
using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using Random = UnityEngine.Random;

public class MapHandler
{
    private static MapData curMap = new();
    public event Action OnGameEnd;
    
    private List<RewardTableData> rewardTableDatas = new List<RewardTableData>();
    public void Init()
    {
        curMap = new ()
        {
            mapId = 1000101,
            chapterId = 1,
            curStageList = new List<int> {10001, 10002, 10103, 10004, 10005, 10206},
            stageId = 10001,
            enemyActor = new List<int>(){20001}
        };
        SetCurMap(1000101);
    }
    
    public static MapData GetMapId()
    {
        return curMap;
    }

    
    public void ShowMapSelect()
    {
        CommandManager.Instance.AddCommand(new PlayerTurnCommand(() => OnGameEnd?.Invoke()), 1f);
        CommandManager.Instance.AddCommand(new PlayerTurnCommand(() =>
            {
                // 현 스테이지 index 
                var curStageIndex = curMap.curStageList.FindIndex(_ => _ == curMap.stageId);
                // 다음 스테이지 고르기
                var stageTable =
                    GameTableManager.Instance._contentStageTableDatas.Find(_ =>
                        _.stage_id == curMap.curStageList[curStageIndex + 1]);

                var firstGroupSelect = stageTable?.advent_cnt_1 ?? 0;
                var firstGroupMap = stageTable?.map_group_1;
                List<int> selectableMap = new List<int>();
                if (firstGroupMap != null)
                {
                    List<int> availableMaps = new List<int>(firstGroupMap);

                    for (int i = 0; i < firstGroupSelect; ++i)
                    {
                        int randomIndex = Random.Range(0, availableMaps.Count);
                        selectableMap.Add(availableMaps[randomIndex]);
                        availableMaps.RemoveAt(randomIndex);
                    }
                }

                var secondGroupSelect = stageTable?.advent_cnt_2 ?? 0;
                var secondGroupMap = stageTable?.map_group_2;
                if (firstGroupMap != null)
                {
                    List<int> availableMaps = new List<int>(secondGroupMap);

                    for (int i = 0; i < secondGroupSelect; ++i)
                    {
                        int randomIndex = Random.Range(0, availableMaps.Count);
                        selectableMap.Add(availableMaps[randomIndex]);
                        availableMaps.RemoveAt(randomIndex);
                    }
                }

                curMap.stageId = stageTable?.stage_id ?? 0;

                if (GameUIManager.Instance.TryGetOrCreate<UI_PopUp_MapSelect>(true, UILayer.LEVEL_4, out var ui))
                {
                    var mapData =
                        GameTableManager.Instance._contentMapTableDatas.FindAll(_ =>
                            selectableMap.Contains(_.map_id ?? 0));
                    ui.SetData(mapData);
                    ui.Show();
                }
            }
        ), 0.1f);
        
        CommandManager.Instance.StartGameCommand();
    }
    
    public void OnClickMapSelect(ContentMapTableData data)
    {
        SetCurMap(data.map_id?? 0);
    }

    public void SetCurMap(int mapId)
    {
        rewardTableDatas.Clear();
        var curMapData = GameTableManager.Instance._contentMapTableDatas.Find(_ => _.map_id == mapId);
        var curMapActor = curMapData.actor_id.ToList();
        int curMapRewardTotalWeight = 0;
        for (int i = 0; i < curMapData.reward_id.Length; i++)
        {
            var reward = GameTableManager.Instance._rewardTable.FindAll(_ => _.reward_id == curMapData.reward_id[i]);
            curMapRewardTotalWeight += reward.Sum(_ => _.prob_weight) ?? 0;
            var randomInt = Random.Range(0, curMapRewardTotalWeight);
            var curMapReward = reward.Find(_ =>
            {
                randomInt -= _.prob_weight ?? 0;
                return randomInt <= 0;
            });
            
            rewardTableDatas.Add(curMapReward);
        }

        
        curMap.enemyActor = curMapActor;
    }
    
    public MapData GetCurMap()
    {
        return curMap;
    }
    
    public List<RewardTableData> GetRewardTableDatas()
    {
        return rewardTableDatas;
    }
}
