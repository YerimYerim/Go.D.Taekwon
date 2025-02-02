using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Manager;
using UnityEngine;

public class GameSupportModuleManager : Singleton<GameSupportModuleManager>
{
    private List<SupportModule> supportModules = new();
    protected override void Awake()
    {
        // base.Awake();
        // supportModules.Clear();
        //
        // var uniqueModuleIds = GameTableManager.Instance._supportModuleTable
        //     .Select(module => module.support_module_id)
        //     .Distinct()
        //     .ToList();
        //
        // supportModules.AddRange(uniqueModuleIds.Select(moduleId => new SupportModule(moduleId ?? 0, 1)));
    }
    public void AddModule(int moduleId)
    {
        var module = GetSupportModule(moduleId);
        if (module == null)
        {
            supportModules.Add(new SupportModule(moduleId, 1));
        }
    }
    public void Add(int moduleId)
    {
        var module = GetSupportModule(moduleId);
        module?.AddLevel(1);
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
        data = GameTableManager.Instance._supportModuleTable.Find(_ => _.support_module_id == moduleId && _.level == level);
    }

    public List<SupportModule> GetSupportModules(int resource)
    {
        var result  = supportModules.FindAll(_=>_.GetApplySource(resource));
        return result;
    }
}
