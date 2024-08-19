using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillApUp : SkillEffectBase, ISkillTargetAPUp
{
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        for (int i = 0; i < targetActor.Count; ++i)
        {
            if(targetActor[i].data is ActorEnemyData)
                ((ActorEnemyData)targetActor[i].data).AddAP(table?.value_1 ?? 0);
            else if (targetActor[i].data is ActorPlayerData)
            {
                var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
                if(battleMode == null)
                    return;
                
                foreach (var source in battleMode.BattleHandler._sources)
                {
                    source.ReduceAP((table?.value_1 ?? 0));
                }
            }
        }
    }

    public override void InitSkillType(SpellEffectTableData data)
    {
        table = data;
        effectId = data?.effect_id ?? 0;
    }

    public override int GetValue()
    {
        return table.value_1 ?? 0;
    }

    public void AddAp(List<GameActor> enemyTarget)
    {
        for (int i = 0; i < enemyTarget.Count; ++i)
        {
            ((ActorEnemyData)enemyTarget[i].data).AddAP(table?.value_1 ?? 0);
        }
    }
}
