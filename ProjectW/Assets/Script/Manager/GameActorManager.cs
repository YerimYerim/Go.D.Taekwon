using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class GameActormanager : Singleton<GameActormanager>
{
    private Dictionary<string, GameActor> actors = new();


    public void AddActors(string prefabName, GameActor actor)
    {
        actors.TryAdd(prefabName, actor);
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

}