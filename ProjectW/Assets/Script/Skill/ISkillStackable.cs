using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillStackable
{
    public abstract void AddStack(GameActor target);
    public abstract void RemoveStack(GameActor target);
    public abstract void DoStackEffect(GameActor target);
}
