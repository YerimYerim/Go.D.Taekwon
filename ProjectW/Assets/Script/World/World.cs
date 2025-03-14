using System;
using UnityEngine;

public class World : MonoBehaviour
{
    Level _level;
    // OnSpawnActor
    public event Action<Actor> OnPreActorSpawned;
    public event Action<Actor> OnActorSpawned;
    
    // OnSpawnActor
    public void AddOnActorSpawnedHandler(Action<Actor> action)
    {
        OnActorSpawned += action;
    }
    
    public void RemoveOnActorSpawnedHandler(Action<Actor> action)
    {
        OnActorSpawned -= action;
    }
    
    public void AddOnPreActorSpawnedHandler(Action<Actor> action)
    {
        OnPreActorSpawned += action;
    }
    
    public void RemoveOnPreActorSpawnedHandler(Action<Actor> action)
    {
        OnPreActorSpawned -= action;
    }
    
    // SpawnActor
    public void SpawnActor(Actor actor, Transform parentsObject)
    {
        OnPreActorSpawned?.Invoke(actor);
        
        actor.transform.SetParent(parentsObject);
        actor.transform.position = parentsObject.position;
        
        OnActorSpawned?.Invoke(actor);
        
        _level.AddActor(actor);

    }

    public void ClearWorld()
    {
        _level.RemoveAllActorsAndRemovedActors();
    }
}