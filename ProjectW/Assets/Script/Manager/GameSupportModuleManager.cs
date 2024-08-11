using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using UnityEngine;

public class GameSupportModuleManager : Singleton<GameSupportModuleManager>
{
    private List<SupportModule> supportModules = new();
    protected override void Init()
    {
        base.Init();
        
        var uniqueModuleIds = GameDataManager.Instance._supportModuleTable
            .Select(module => module.support_module_id)
            .Distinct()
            .ToList();
        
        supportModules.AddRange(uniqueModuleIds.Select(moduleId => new SupportModule(moduleId ?? 0, 1)));
    }

    private SupportModule GetSupportModule(int moduleId)
    {
        return supportModules.Find(module => module.GetModuleId() == moduleId);
    }
    
    public void AddLevel(int moduleId, int addLevel)
    {
        var module = GetSupportModule(moduleId);
        module?.AddLevel(addLevel);
    }
    
    public void SetLevel(int moduleId, int level)
    {
        var module = GetSupportModule(moduleId);
        module?.SetLevel(level);
    }
    
    public Sprite GetSupportModuleImage(int moduleId)
    {
        var module = GetSupportModule(moduleId);
        return module?.GetImage();
    }
    
    public string GetSupportModuleName(int moduleId)
    {
        var module = GetSupportModule(moduleId);
        return module?.GetName();
    }
    
    public string GetSupportModuleDesc(int moduleId)
    {
        var module = GetSupportModule(moduleId);
        return module?.GetDesc();
    }
    
    public int GetSupportModuleEffectValue(int moduleId, SUPPORT_MODULE_EFFECT effect)
    {
        var module = GetSupportModule(moduleId);
        return module?.GetEffectValue(effect) ?? 0;
    }

    public void SetSupportModuleData(int moduleId, int level, out SupportModuleTableData data)
    {
        data = GameDataManager.Instance._supportModuleTable.Find(_ => _.support_module_id == moduleId && _.level == level);
    }
    

}
