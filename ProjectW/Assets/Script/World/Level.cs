using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private List<Actor> RemovedActors { get; set; }
    private List<Actor> Actors { set; get; }
    
    private void Awake()
    {
        Actors = new List<Actor>();
        RemovedActors = new List<Actor>();
    }
    
    public void AddActor(Actor actor)
    {
        Actors.Add(actor);
    }
    
    public void RemoveActor(Actor actor)
    {
        Actors.Remove(actor);
        RemovedActors.Add(actor);
    }
    
    public void RemoveAllActors()
    {
        foreach (var actor in Actors)
        {
            RemovedActors.Add(actor);
        }
        Actors.Clear();
    }
    
    public void RemoveAllRemovedActors()
    {
        foreach (var actor in RemovedActors)
        {
            Destroy(actor.gameObject);
        }
        RemovedActors.Clear();
    }
    
    public void RemoveAllActorsAndRemovedActors()
    {
        RemoveAllActors();
        RemoveAllRemovedActors();
    }
    
}
