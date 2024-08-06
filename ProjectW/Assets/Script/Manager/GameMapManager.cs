using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using UnityEngine;

public class GameMapManager : Singleton<GameMapManager>
{
    private MapData curMap = new ();
    public string actorParent = "Actors";


    protected override void Init()
    {
        base.Init();
        curMap.mapId = 101;
    }
    public void SpawnActors()
    {
        var curMapData = GameDataManager.Instance._contentMapTableDatas.Find(_ => _.map_id == curMap.mapId);
        var curMapActor = curMapData.actor_id.ToList();
        var actorMonsterDatas = GameDataManager.Instance._actorDatas.FindAll(_ => _.actor_type == ACTOR_TYPE.ACTOR_TYPE_MONSTER
            && curMapActor.Contains(_.actor_id ?? 0) == true);
        for (int i = 0; i < actorMonsterDatas.Count; ++i)
        {
            var actorPrefab = GameUtil.GetActorPrefab(actorMonsterDatas[i]?.rsc_id ?? 0);
            actorPrefab.transform.SetParent(GameObject.Find(actorParent).transform);
            var monsterData = GameDataManager.Instance._monsterTableDatas.Find(_ => _.actor_id == actorMonsterDatas[i].actor_id);
            var enemyData = new ActorEnemyData();
            enemyData.Init(monsterData?.stat_hp?? 0);
            enemyData.InitAP(monsterData?.skill_pattern_group ?? 0);
            
            GameBattleManager.Instance.SpawnEnemy(GameActormanager.Instance.GetActor(actorPrefab.name));
            GameBattleManager.Instance.SetEnemyData(i,enemyData);
        }
        GameBattleManager.Instance.UpdateEnemyHp();
    }
}
