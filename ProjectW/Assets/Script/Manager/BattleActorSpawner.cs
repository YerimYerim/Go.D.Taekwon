using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class BattleActorSpawner
{
    private Dictionary<string, GameActor> actors = new();
    private List<GameActor> enemy = new();
    
    public void AddActors(GameActor actor)
    {
        actors.TryAdd(actor.name, actor);
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
            actors.Remove(key);
        }
    }
    
    public GameActor SpawnActorPrefab(ActorTableData actorTableData, string parentsObjectName, string actorName = "")
    {
        var rscTableData = GameDataManager.Instance._actorRscDatas.Find(_ => _.rsc_id == actorTableData.rsc_id);
        var prefab = GameResourceManager.Instance.GetLoadActorPrefab(rscTableData.actor_rsc_prefab);
        var parentsObjectTr = GameObject.Find(parentsObjectName).transform;
        if(parentsObjectTr == null)
            return null;
        
        prefab.transform.SetParent(parentsObjectTr);
        prefab.transform.position = parentsObjectTr.position;
        prefab.name = actorName.Equals(string.Empty) == false ? actorName : prefab.name;
        
        var actor = prefab.GetComponent<GameActor>();
        AddActors(actor);
        
        return actor;
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
    public void RemoveAllEnemy()
    {
        enemy.Clear();
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

    public void UpdateBuff()
    {
        for (int i = 0; i < enemy.Count; ++i)
        {
            enemy[i].UpdateDebuff();
            enemy[i].UpdateBuff();
            enemy[i].OnUpdateHp(enemy[i].data);
        }
    }
}
