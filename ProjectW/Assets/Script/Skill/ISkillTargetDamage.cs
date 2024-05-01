using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillTargetDamage
{
    protected abstract void DoDamage(GameActor enemy);
}
