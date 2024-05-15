using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillTargetHeal
{
    public abstract void DoHeal(List<GameActor> targetActor);
}
