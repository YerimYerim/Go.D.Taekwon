using System;
using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class SupportModule
{
    private int moduleId;
    private int curLevel;

    // support module effect , value
    private Dictionary<SUPPORT_MODULE_EFFECT, int> effect;
    private Sprite img;
    private string moduleName;
    private string moduleDesc;

    public SupportModule(int id, int level)
    {
        moduleId = id;
        curLevel = level;
        effect = new Dictionary<SUPPORT_MODULE_EFFECT, int>();
        SetSupportModuleData(id, level, out var data);
        SetResource(data);
    }

    private void SetSupportModuleData(int id, int level, out List<SupportModuleTableData> data)
    {
        data = GameDataManager.Instance._supportModuleTable.FindAll(_ => _.support_module_id == id && _.level == level);
        for (int i = 0; i < data.Count; i++)
        {
            int? effectValue = data[i].effect_value;
            SUPPORT_MODULE_EFFECT? supportModuleEffect = data[i].support_module_effect;
            int? sourceID = data[i].source_id;
            if (effectValue != null && supportModuleEffect != null && sourceID != null)
            {
                UpdateEffect(supportModuleEffect.Value, effectValue.Value);
                ApplyEffect(sourceID.Value, supportModuleEffect.Value, effectValue.Value);
            }
        }
    }

    private void SetResource(List<SupportModuleTableData> data)
    {
        img = GameResourceManager.Instance.GetImage(data[^1].support_module_img);
        moduleName = data[^1].support_module_name;
        moduleDesc = data[^1].support_module_desc;
    }

    public void SetLevel(int level)
    {
        this.curLevel = level;
        effect.Clear();
        SetSupportModuleData(moduleId, curLevel, out var data);
        SetResource(data);
    }

    public void AddLevel(int addLevel)
    {
        this.curLevel += addLevel;
        effect.Clear();
        SetSupportModuleData(moduleId, curLevel, out var data);
        SetResource(data);
    }

    public int GetLevel()
    {
        return curLevel;
    }

    public void UpdateEffect(SUPPORT_MODULE_EFFECT effectType, int value)
    {
        effect[effectType] = value;
    }

    public int GetEffectValue(SUPPORT_MODULE_EFFECT effectType)
    {
        return effect.TryGetValue(effectType, out var value) ? value : 0;
    }

    public Sprite GetImage()
    {
        return img;
    }

    public string GetName()
    {
        return moduleName;
    }

    public string GetDesc()
    {
        return moduleDesc;
    }

    public int GetModuleId()
    {
        return moduleId;
    }
    
    public void ApplyEffect(int resourceID ,SUPPORT_MODULE_EFFECT effectType, int value)
    {
        var source = GameBattleManager.Instance._sources.Find(_ => _.GetSourceId() == resourceID);
        // apply effect
        switch (effectType)
        {
            case SUPPORT_MODULE_EFFECT.SUPPORT_MODULE_EFFECT_REDUCE_PRODUCT_AP:
                source.SetTableMaxAp();
                source.SetMaxAp(source.GetMaxAp() - value);
                break;
            case SUPPORT_MODULE_EFFECT.SUPPORT_MODULE_EFFECT_INCREASE_PRODUCT_AP:
                source.SetTableMaxAp();
                source.SetMaxAp(source.GetMaxAp() + value);
                break;
            case SUPPORT_MODULE_EFFECT.SUPPORT_MODULE_EFFECT_REDUCE_PRODUCT_VALUE:
                source.SetTableProductionAmount();
                source.SetProductionAmount(source.GetProductionAmount() - value);
                break;
            case SUPPORT_MODULE_EFFECT.SUPPORT_MODULE_EFFECT_INCREASE_PRODUCT_VALUE:
                source.SetTableProductionAmount();
                source.SetProductionAmount(source.GetProductionAmount() - value);
                break;
            case SUPPORT_MODULE_EFFECT.SUPPORT_MODULE_EFFECT_CHANGE_PRODUCT_SPELL:
                source.SetTableProductionSpellId();
                source.SetProductionSpellId(value);
                break;
        }
    }
}