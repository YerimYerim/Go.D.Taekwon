using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameDataManager : Script.Manager.Singleton<GameDataManager>
{
    private readonly string[] _pageJsonFileNames = {"config", "spell", "string" };
    internal List<ConfigTableData> _configTableData = new();
    internal List<SpellTableData> _spellData = new();
    internal List<StringTableData> _stringDatas = new();


    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadData()
    {
        _configTableData = ReadJsonFiles<ConfigTableData>(_pageJsonFileNames[0]);
        _spellData = ReadJsonFiles<SpellTableData>(_pageJsonFileNames[1]);
        _stringDatas = ReadJsonFiles<StringTableData>(_pageJsonFileNames[2]);
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
