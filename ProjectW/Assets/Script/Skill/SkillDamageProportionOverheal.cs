
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillDamageProportionOverheal : SkillEffectBase, ISkillTurnSkill, ISkillOnHeal
{
    private GameActor _myActor;
    public override void DoSkill(List<GameActor> targetActor, GameActor myActor)
    {
        _myActor = myActor;
        _myActor.data.OnHeal += OnHeal;
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

    public void DoTurnSkill(GameActor target)
    {
        --remainTurn;
    }

    public int GetRemainTime()
    {
        return remainTurn;
    }

    public void DoTurnEndSkill(GameActor target)
    {
        _myActor.data.OnHeal -= OnHeal;
        _myActor = null;
    }

    public void OnHeal(int heal, int overHeal)
    {
        var gameMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        switch (table.target)
        {
            case TARGET_TYPE.TARGET_TYPE_ENEMY:
            {
                int enemyCount = gameMode.ActorSpawner.GetEnemyCount();
                var enemyActor = gameMode.ActorSpawner.GetEnemy(Random.Range(0, enemyCount));
                enemyActor.data.TakeDamage(Mathf.CeilToInt(GetValue() * 0.01f * overHeal));
                
            } break;
            case TARGET_TYPE.TARGET_TYPE_ENEMY_ALL:
            {
                int enemyCount = gameMode.ActorSpawner.GetEnemyCount();
                for (int i = 0; i < enemyCount; i++)
                {
                    var enemyActor = gameMode.ActorSpawner.GetEnemy(i);
                    enemyActor.data.TakeDamage(Mathf.CeilToInt(GetValue() * 0.01f * overHeal));
                }
            } break;
            case null:
                break;
        }
    }
}
