using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillIgnoreDamage
{
    public abstract void SetIgnoreDamage(GameActor target);
    public abstract void RemoveIgnoreDamage(GameActor target);
}
