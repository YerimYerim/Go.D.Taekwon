using System.Collections.Generic;
using UnityEngine;

public class ActorHandler
{
    private Dictionary<string, GameActor> actors = new();
    private List<GameActor> enemy = new();
    
    public GameActor SpawnActor(ActorTableData actorTableData, string parentsObjectName, string actorName = "")
    {
        var actor = ActorSpawnFactory.SpawnActorPrefab(actorTableData, parentsObjectName, actorName);
        actors.TryAdd(actor.name, actor);
        return actor;
    }

    private void RemoveActors(string prefabName)
    {
        actors.Remove(prefabName, out var actor);
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
        if (map.mapType == MAP_TYPE.MAP_TYPE_SPECIAL)
        {
            return;
        }
        for (int i = 0; i < map.enemyActor.Count; ++i)
        {
            var actorData = GameTableManager.Instance._actorDatas.Find(data => data.actor_id == map.enemyActor[i] && data.actor_type == ACTOR_TYPE.ACTOR_TYPE_MONSTER);
    
            string enemyName = $"{GameUtil.ENEMY_PARENT_NAME}_{i + 1}";
            GameActor actor = SpawnActor(actorData, enemyName, enemyName);
            SpawnEnemy(actor);
    
            MonsterTableData monsterData = GameTableManager.Instance._monsterTableDatas.Find(data => data.actor_id == actorData.actor_id);
            SetEnemyData(i,  new ActorEnemyData(monsterData));
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
        return enemy.Count > i ? enemy[i] : null;
    }
        
    public List<GameActor> GetEnemyAll()
    {
        return enemy;
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
        if(enemy.Count <= 0)
        {
            return;
        }

        foreach (var e in enemy)
        {
            e.UpdateTurnSkill();
            e.OnUpdateHp(e.data);
        }
    }
    
    public List<GameActor> GetEnemyData()
    {
        return enemy;
    }

    public void OnDispose()
    {
        foreach (KeyValuePair<string, GameActor> actor in actors)
        {
            Object.Destroy(actor.Value.gameObject);
        }
        actors.Clear();
        enemy.Clear();
    }
}
