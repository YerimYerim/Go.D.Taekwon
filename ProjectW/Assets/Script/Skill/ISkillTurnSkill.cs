using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 해당 ISkillTurnSkill 은 SkillEffectBase이 포함된 클래스에서만 사용가능


public interface ISkillTurnSkill
{
    /// <summary>
    ///  턴마다 시행되는 스킬
    /// </summary>
    /// <param name="enemy"></param>
    abstract void DoTurnSkill(GameActor enemy);
    abstract int GetRemainTime();
}
