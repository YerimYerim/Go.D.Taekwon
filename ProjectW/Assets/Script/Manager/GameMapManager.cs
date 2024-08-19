using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using UnityEngine;

public class GameMapManager : Singleton<GameMapManager>
{
    private MapData curMap = new ();
    
    public string enemy = "Enemy";
    public string actor = "Actors";


    protected override void Init()
    {
        base.Init();
        
        curMap.mapId = 1000101;
        curMap.chapterId = 1;
        curMap.curStageList = new List<int> {10001, 10002, 10103, 10004, 10005, 10206};
        curMap.stageId = 10001;
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
            var actorPrefab = GameUtil.GetActorPrefab(actorMonsterDatas[i]?.rsc_id ?? 0);
            actorPrefab.name = actorPrefab.name + "_" + (i + 1);
            
            GameActormanager.Instance.AddActors(actorPrefab.name, actorPrefab);
            Transform prefabTransform = actorPrefab.transform;
            
            var prefabTransformParent = GameObject.Find(enemy + "_" + (i + 1)).transform;
            prefabTransform.SetParent(prefabTransformParent);
            prefabTransform.position = prefabTransformParent.position;

            var monsterData = GameDataManager.Instance._monsterTableDatas.Find(_ => _.actor_id == actorMonsterDatas[i].actor_id);
            var enemyData = new ActorEnemyData();
            
            enemyData.Init(monsterData?.stat_hp?? 0);
            enemyData.InitAP(monsterData?.skill_pattern_group ?? 0);


            if(battleMode == null)
                return;
            battleMode.BattleHandler.SpawnEnemy(GameActormanager.Instance.GetActor(actorPrefab.name));
            battleMode.BattleHandler.SetEnemyData(i,enemyData);
        }
        if(battleMode == null)
            return;
        battleMode.BattleHandler.UpdateEnemyHp();
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
    public void RemoveActorsAll()
    {
        GameActormanager.Instance.RemoveAllMonsterActors();
        var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        battleMode.BattleHandler.RemoveAllEnemy();
    }
    public void OnClickMapSelect(ContentMapTableData data)
    {
        curMap.mapId = data.map_id ?? 0;
        RemoveActorsAll();
        SpawnActors();
    }
}
