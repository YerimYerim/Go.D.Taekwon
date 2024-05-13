using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillTargetBuff
{
    abstract void DoBuff(GameActor enemy);
}
