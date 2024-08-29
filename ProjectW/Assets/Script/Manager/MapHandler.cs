using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using UnityEngine;

public class MapHandler
{
    private MapData curMap = new();
    
    public void Init()
    {
        curMap = new ()
        {
            mapId = 1000101,
            chapterId = 1,
            curStageList = new List<int> {10001, 10002, 10103, 10004, 10005, 10206},
            stageId = 10001
        };
    }

    public void SpawnActors()
    {
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        var curMapData = GameDataManager.Instance._contentMapTableDatas.Find(_ => _.map_id == curMap.mapId);
        var curMapActor = curMapData.actor_id.ToList();
        List<ActorTableData> actorMonsterDatas = new();
        for (int i = 0; i < curMapActor.Count; ++i)
        {
            var actorData = GameDataManager.Instance._actorDatas.Find(_ => _.actor_id == curMapActor[i] && _.actor_type == ACTOR_TYPE.ACTOR_TYPE_MONSTER);
            actorMonsterDatas.Add(actorData);
        }
        
        for (int i = 0; i < actorMonsterDatas.Count; ++i)
        {
            string enemyName = GameUtil.ENEMY_PARENT_NAME + "_" + (i + 1);
            
            var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
            var actorSpawner = gameBattleMode?.BattleActorSpawner;
        
            if(actorSpawner == null)
                return;
            
            var actorPrefab = actorSpawner.SpawnActorPrefab(actorMonsterDatas[i], enemyName, enemyName);

            var monsterData = GameDataManager.Instance._monsterTableDatas.Find(_ => _.actor_id == actorMonsterDatas[i].actor_id);
            var enemyData = new ActorEnemyData();
            
            enemyData.Init(monsterData?.stat_hp?? 0);
            enemyData.InitAP(monsterData?.skill_pattern_group ?? 0);

            if(battleMode == null)
                return;
            battleMode.BattleActorSpawner.SpawnEnemy(actorSpawner.GetActor(actorPrefab.name));
            battleMode.BattleActorSpawner.SetEnemyData(i,enemyData);
        }

        battleMode?.BattleActorSpawner.UpdateEnemyHp();
    }

    public void SetMap(int mapId)
    {
        curMap.mapId = mapId;
        SpawnActors();
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

        var SecondGroupSelect = stageTable?.advent_cnt_2 ?? 0;
        var SecondGroupMap = stageTable?.map_group_2;
        if (firstGroupMap != null)
        {
            List<int> availableMaps = new List<int>(SecondGroupMap);

            for (int i = 0; i < SecondGroupSelect; ++i)
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
        var actorSpawner = battleMode?.BattleActorSpawner;
        
        if(actorSpawner == null)
            return;
        
        actorSpawner.RemoveAllMonsterActors();
        battleMode.BattleActorSpawner.RemoveAllEnemy();
    }
    public void OnClickMapSelect(ContentMapTableData data)
    {
        curMap.mapId = data.map_id ?? 0;
        RemoveActorsAll();
        SpawnActors();
    }
}
