using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor
{
    private GameActor _actorObject;
    private ActorDataBase _data;
    
    public Actor(GameActor actorObject, ActorDataBase data)
    {
        _actorObject = actorObject;
        _data = data;
    }
    public void Instance()
    {
        
    }
}
