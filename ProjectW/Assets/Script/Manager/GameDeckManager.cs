using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class GameDeckManager : Singleton<GameDeckManager>
{
    public class SpellData
    {
        public SpellTableData tableData
        {
            get;
        }
        private List<SpellEffectTableData> effectData = new();

        public SpellData(SpellTableData data)
        {
            tableData = data;
            effectData.Clear();
            foreach (var effect in data.spell_effect)
            {
                var effectTableData = GameTableManager.Instance._spelleffectDatas.Find(_ => _.effect_id == effect);
                effectData.Add(effectTableData);
            }
        }
    }

    private List<int> _onHandSpellKeys = new();
    private List<int> _newPoolSpells = new();
    private List<int> _graveYardSpells = new();

    protected override void Awake()
    {
        base.Awake();

        _newPoolSpells.Clear();
        _newPoolSpells.AddRange(new[]
        {
            1, 2, 3, 4, 1, 2, 3, 4
        });
        _graveYardSpells.Clear();
    }

    public void OnSpellCombine(int a, int b)
    {
        
    }
    
    public void OnAttack(int spellID)
    {
        
    }

    public void OnDraw()
    {
        
    }
    

}
