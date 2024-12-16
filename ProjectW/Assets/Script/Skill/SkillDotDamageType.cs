using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDotDamageType : SkillEffectBase, ISkillTargetDamage, ISkillTurnSkill
{
    GameActor myActor;
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        myActor = this.myActor;
        for (int i = 0; i < targetActor.Count; ++i)
        {
            targetActor[i].data.AddTurnSkill(this);
        }
    }

    public override void InitSkillType(SpellEffectTableData data)
    {
        table = data;
        effectId = data?.effect_id ?? 0;
        remainTurn = data?.remain_turn_count ??0;
    }

    public override int GetValue()
    {
        return table.value_1 ?? 0;
    }
    
    public void DoTurnSkill(GameActor enemy)
    {
        DoDamage(enemy);
        --remainTurn;
    }

    public int GetRemainTime()
    {
        return remainTurn;
    }

    public void DoTurnEndSkill(GameActor target)
    {
        
    }

    public void DoDamage(GameActor enemy)
    {
        var attackStat = myActor.data.GetAttackStat();
        enemy.data.TakeDamage(GetValue()  * Mathf.CeilToInt(1 + attackStat.DamageUpPer* 0.01f + attackStat.DamageUpFixed));
        myActor?.data.GiveDamage(GetValue()  * Mathf.CeilToInt(1 + attackStat.DamageUpPer * 0.01f + attackStat.DamageUpFixed));
    }
}
