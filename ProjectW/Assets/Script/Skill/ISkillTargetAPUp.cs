using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillTargetAPUp
{
    abstract void AddAp(List<GameActor> enemyTarget);
}
