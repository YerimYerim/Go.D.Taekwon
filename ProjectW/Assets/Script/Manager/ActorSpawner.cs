using System.Collections.Generic;
using UnityEngine;

public class ActorSpawner
{
    private Dictionary<string, GameActor> actors = new();
    private List<GameActor> enemy = new();
    
    public GameActor SpawnActor(ActorTableData actorTableData, string parentsObjectName, string actorName = "")
    {
        var actor = ActorSpawnFactory.SpawnActorPrefab(actorTableData, parentsObjectName, actorName);
        actors.TryAdd(actor.name, actor);
        return actor;
    }

    public void RemoveActors(string prefabName)
    {
        if (actors.TryGetValue(prefabName, out var actor))
        {
            actors.Remove(prefabName);
        }
    }

    public bool IsContainActor(string key)
    {
        return actors.ContainsKey(key);
    }

    public GameActor GetActor(string key)
    {
        if (actors.TryGetValue(key, out var actor))
        {
            return actor;
        };
        return null;
    }

    public void RemoveAllMonsterActors()
    {
        List<string> keysToRemove = new List<string>();
        foreach (KeyValuePair<string, GameActor> actor in actors)
        {
            if (actor.Value.data is ActorEnemyData)
            {
                Object.Destroy(actor.Value.gameObject);
                keysToRemove.Add(actor.Key);
            }
        }

        foreach (string key in keysToRemove)
        {
            RemoveActors(key);
        }
        enemy.Clear();
    }
    

    /// <summary>
    /// 적 스폰
    /// </summary>
    /// <param name="map"></param>
    public void SpawnEnemyActors(MapData map)
    {
        List<ActorTableData> actorMonsterDatas = new();
        for (int i = 0; i < map.enemyActor.Count; ++i)
        {
            var actorData = GameTableManager.Instance._actorDatas.Find(_ => _.actor_id ==  map.enemyActor[i] && _.actor_type == ACTOR_TYPE.ACTOR_TYPE_MONSTER);
            actorMonsterDatas.Add(actorData);
        }
        
        for (int i = 0; i < map.enemyActor.Count; ++i)
        {
            string enemyName = GameUtil.ENEMY_PARENT_NAME + "_" + (i + 1);
            var actorPrefab =  SpawnActor(actorMonsterDatas[i], enemyName, enemyName);
            
            var monsterData = GameTableManager.Instance._monsterTableDatas.Find(_ => _.actor_id == actorMonsterDatas[i].actor_id);
            var enemyData = new ActorEnemyData();
            
            enemyData.Init(monsterData?.stat_hp?? 0);
            enemyData.InitAP(monsterData?.skill_pattern_group ?? 0);

            SpawnEnemy(GetActor(actorPrefab.name));
            SetEnemyData(i,enemyData);
        }

        UpdateEnemyHp();
    }
    
    public void UpdateEnemyHp()
    {
        for (int i = 0; i < enemy.Count; ++i)
        {
            enemy[i].OnUpdateHp(enemy[i].data);
        }
    }

    public void SetEnemyData(int i, ActorEnemyData enemyData)
    {
        enemy[i].data = enemyData;
    }

    public void SpawnEnemy(GameActor actorPrefab)
    {
        enemy.Add(GetActor(actorPrefab.name));
    }

    public ActorEnemyData GetEnemyData(int i)
    {
        return enemy[i].data as ActorEnemyData;
    }
    
    public GameActor GetEnemy(int i)
    {
        return enemy[i];
    }
    
    public int GetEnemyCount()
    {
        return enemy.Count;
    }
    
    
    public bool IsEnemyTurn()
    {
        for (int i = 0; i < enemy.Count; ++i)
        {
            if (((ActorEnemyData)enemy[i].data).IsCanUseSkill())
            {
                return true;
            }
        }
        return false;
    }
    
    public bool IsAllEnemyDead()
    {
        for (int i = 0; i < enemy.Count; ++i)
        {
            if (enemy[i].data.GetHp() > 0)
            {
                return false;
            };
        }

        return true;
    }
    
    public void MinusAP(int minusAP)
    {
        for (int i = 0; i < enemy.Count; ++i)
        {
            var enemyData = (ActorEnemyData)enemy[i].data;
            enemyData.MinusAP(minusAP);
        }
    }

    public void UpdateTurnSkill()
    {
        for (int i = 0; i < enemy.Count; ++i)
        {
            enemy[i].UpdateTurnSkill();
            enemy[i].OnUpdateHp(enemy[i].data);
        }
    }
    
    public List<GameActor> GetEnemyData()
    {
        return enemy;
    }
}
