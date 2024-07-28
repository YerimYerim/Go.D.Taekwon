using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameDataManager : Script.Manager.Singleton<GameDataManager>
{
    private readonly string[] _pageJsonFileNames = {"config", "spell", "string", "spell_combine", 
        "spell_effect", "actor_rsc", "actor", "playable_character", "monster","skill_pattern_group", "skill_group",
        "spell_source"
    };
    
    internal List<ConfigTableData> _configTableData = new();
    internal List<SpellTableData> _spellData = new();
    internal List<StringTableData> _stringDatas = new();
    internal List<SpellCombineTableData> _spellCombineDatas = new();
    internal List<SpellEffectTableData> _spelleffectDatas = new();
    internal List<ActorRscTableData> _actorRscDatas = new();
    internal List<ActorTableData> _actorDatas = new();
    internal List<PlayableCharacterTableData> _playableCharacterDatas = new();
    internal List<MonsterTableData> _monsterTableDatas = new();
    internal List<SkillPatternGroupTableData> _patternGroupTableDatas= new();
    internal List<SkillGroupTableData> _skillGroupTableDatas= new();
    internal List<SpellSourceTableData> _spellSourceTableDatas= new();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Init()
    {
        LoadData();
    }
    public void LoadData()
    {
        _configTableData = ReadJsonFiles<ConfigTableData>(_pageJsonFileNames[0]);
        _spellData = ReadJsonFiles<SpellTableData>(_pageJsonFileNames[1]);
        _stringDatas = ReadJsonFiles<StringTableData>(_pageJsonFileNames[2]);
        _spellCombineDatas = ReadJsonFiles<SpellCombineTableData>(_pageJsonFileNames[3]);
        _spelleffectDatas = ReadJsonFiles<SpellEffectTableData>(_pageJsonFileNames[4]);
        _actorRscDatas = ReadJsonFiles<ActorRscTableData>(_pageJsonFileNames[5]);
        _actorDatas = ReadJsonFiles<ActorTableData>(_pageJsonFileNames[6]);
        _playableCharacterDatas = ReadJsonFiles<PlayableCharacterTableData>(_pageJsonFileNames[7]);
        _monsterTableDatas = ReadJsonFiles<MonsterTableData>(_pageJsonFileNames[8]);
        _patternGroupTableDatas = ReadJsonFiles<SkillPatternGroupTableData>(_pageJsonFileNames[9]);
        _skillGroupTableDatas = ReadJsonFiles<SkillGroupTableData>(_pageJsonFileNames[10]);
        _spellSourceTableDatas = ReadJsonFiles<SpellSourceTableData>(_pageJsonFileNames[11]);
    }

    private static List<T> ReadJsonFiles<T>(string fileName)
    {
        List<T> dataList = new List<T>();
        var jsonData = Resources.Load<TextAsset>($"Json/{fileName}");
        try
        {
            List<T> data = JsonConvert.DeserializeObject<List<T>>(jsonData.text);

            if (data != null)
            {
                dataList.AddRange(data);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return dataList;
    }
    
    public object GetValueConfigData(string key)
    {
        var tableData = _configTableData.Find(_ => _.config_id == key);
        switch (tableData.data_type)
        {
            case "string[]":
            {
                return GetStringList(tableData);
            }
            case "int":
            {
                return (int)long.Parse(tableData.value);
            }
            default:
                return string.Empty;
        }
    }

    private string[] GetStringList(ConfigTableData tableData)
    {
        string values = tableData.value;
        string[] strings = values.Split(", ");
        return strings;
    }
    
    public object GetValueConfigData(ConfigTableData tableData)
    {
        switch (tableData.data_type)
        {
            case "string[]":
            {
                return GetStringList(tableData);
            }
            case "int":
            {
                return (int)long.Parse(tableData.value);
            }
            default:
                return string.Empty;
        }
    }

}
