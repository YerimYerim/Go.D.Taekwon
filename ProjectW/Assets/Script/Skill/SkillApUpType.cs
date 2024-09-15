using System;
using System.Collections.Generic;

public class SkillApUp : SkillEffectBase, ISkillTargetAPUp
{
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        AddAp(targetActor);
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
            switch (enemyTarget[i].data)
            {
                case ActorEnemyData:
                {
                    ((ActorEnemyData)enemyTarget[i].data).AddAP(table?.value_1 ?? 0);
                } break;
                case ActorPlayerData:
                {
                    var battleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
                    if(battleMode == null)
                        return;
                
                    foreach (var source in battleMode.BattleHandler._sources)
                    {
                        source.AddAP((table?.value_1 ?? 0));
                    }
                } break;
            }
        }
    }
}
