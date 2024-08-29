using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using UnityEngine;

public class MapHandler
{
    private static MapData curMap = new();
    
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
    }
    
    public static MapData GetMapId()
    {
        return curMap;
    }


    public void SetMap(int mapId)
    {
        curMap.mapId = mapId;
    }

    public void ShowMapSelect()
    {
        // 현 스테이지 index 
        var curStageIndex = curMap.curStageList.FindIndex(_ => _ == curMap.stageId);
        // 다음 스테이지 고르기
        var stageTable = GameDataManager.Instance._contentStageTableDatas.Find(_ => _.stage_id == curMap.curStageList[curStageIndex + 1]);

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
        
        // GameDataManager.Instance._contentMapTableDatas;
        //GameDataManager.Instance._contentChapterTableDatas;
        
        curMap.stageId = stageTable?.stage_id ?? 0;
        if (GameUIManager.Instance.TryGetOrCreate<UI_PopUp_MapSelect>(true, UILayer.LEVEL_4, out var ui))
        {
            var mapData = GameDataManager.Instance._contentMapTableDatas.FindAll(_ => selectableMap.Contains(_.map_id?? 0));
            ui.SetData(mapData);
            ui.Show();
        };
    }

    private void RemoveActorsAll()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var actorSpawner = battleMode?.ActorSpawner;
        
        if(actorSpawner == null)
            return;
        
        actorSpawner.RemoveAllMonsterActors();
        battleMode.ActorSpawner.RemoveAllEnemy();
    }
    public void OnClickMapSelect(ContentMapTableData data)
    {
        SetCurMap(data.map_id?? 0);
        RemoveActorsAll();
       
    }

    public void SetCurMap(int mapId)
    {
        var curMapData = GameDataManager.Instance._contentMapTableDatas.Find(_ => _.map_id == mapId);
        var curMapActor = curMapData.actor_id.ToList();
        curMap.enemyActor = curMapActor;
    }
    
    public MapData GetCurMap()
    {
        return curMap;
    }
}
