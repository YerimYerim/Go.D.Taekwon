using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIApGauge : UIBase
{
    // 시작과 끝을 다섯칸으로 나눠준다
    [SerializeField] private Transform[] _apGaugePos;
    
    [SerializeField] UIAPGaugeIcon _apSourcePrefab;
    [SerializeField] UIAPGaugeIconMonster _apEnemyPrefab;
    
    [SerializeField] Transform _apSourceParentsPos;
    [SerializeField] Transform _apEnemyParentsPos;
    
    Dictionary<int, List<UIAPGaugeIcon>> sourceDic = new();
    Dictionary<int, List<UIAPGaugeIconMonster>> monsterDic = new();
    
    // ap , spell
    private readonly List<UIAPGaugeIcon> _spellSources = new();
    private List<UIAPGaugeIconMonster> _monsters = new();
    
    private void SpawnSourceIcon(GameBattleMode gameBattleMode)
    {
        foreach (var spell in gameBattleMode.BattleHandler._sources)
        {
            var obj = Instantiate(_apSourcePrefab, _apGaugePos[^1].position, Quaternion.identity, _apSourceParentsPos);
            var uiApGaugeIcon = obj.GetComponent<UIAPGaugeIcon>();
            uiApGaugeIcon.SetSpellSource(spell);
            _spellSources.Add(uiApGaugeIcon);
        }

        SetSpellDictionary();
        _apSourcePrefab.gameObject.SetActive(false);
    }

    private void SpawnMonsterIcon(GameBattleMode gameBattleMode)
    {
        foreach (var monsterData in gameBattleMode.ActorSpawner.GetEnemyData())
        {
            var obj = Instantiate(_apEnemyPrefab, _apGaugePos[^1].position, Quaternion.identity, _apEnemyParentsPos);
            var uiMonster = obj.GetComponent<UIAPGaugeIconMonster>();
            uiMonster.SetData(monsterData);
            _monsters.Add(uiMonster);
        }

        SetMonsterDictionary();
        _apEnemyPrefab.gameObject.SetActive(false);
    }
    
    public void Init()
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        ResetAll();
        SpawnSourceIcon(gameBattleMode);
        SpawnMonsterIcon(gameBattleMode);
        UpdateUI(false);
        StartCoroutine(InitCoroutine());
    }

    private void ResetSource()
    {
        foreach (var source in _spellSources)
        {
            Destroy(source.gameObject);
        }
        sourceDic.Clear();
        _spellSources.Clear();
    }
    
    private void ResetMonster()
    {
        foreach (var monster in _monsters)
        {
            Destroy(monster.gameObject);
        }
        monsterDic.Clear();
        _monsters.Clear();
    }

    public void ResetAll()
    {
        ResetSource();
        ResetMonster();
    }
    
    private IEnumerator InitCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateUI();
    }

    public void UpdateUI(bool isSmooth = true)
    {
        SetSpellDictionary();
        UpdateSourceUI(isSmooth);
        SetMonsterDictionary();
        UpdateMonsterUI(isSmooth);
    }

    private void UpdateUI<T>(Dictionary<int, List<T>> dictionary, List<T> sources, Transform prefabTransform, bool isSmooth) where T : UIAPGaugeIconBase
    {
        for (int i = 0; i < sources.Count; i++)
        {
            int remainAP = sources[i].GetRemainSpellAP();
            int sameAPCount = dictionary.TryGetValue(remainAP, out var value) ? value.Count : 1;
            int myIndex = dictionary.TryGetValue(remainAP, out var value1) ? value1.FindIndex(_ => _ == sources[i]) : 0;

            var position = GetPosition(sameAPCount, remainAP, myIndex, prefabTransform.position.y);
            if (isSmooth)
                sources[i].SetPositionSmooth(position);
            else
                sources[i].SetPosition(position);
        }
    }

    private void UpdateSourceUI(bool isSmooth)
    {
        UpdateUI(sourceDic, _spellSources, _apSourcePrefab.transform, isSmooth);
    }

    private void UpdateMonsterUI(bool isSmooth)
    {
        UpdateUI(monsterDic, _monsters, _apEnemyPrefab.transform, isSmooth);
    }

    #region SetDictionary

    private void SetDictionary<T>(Dictionary<int, List<T>> dictionary, List<T> sources) where T : UIAPGaugeIconBase
    {
        dictionary.Clear();
        foreach (var source in sources)
        {
            int remainAP = source.GetRemainSpellAP();
            if (!dictionary.ContainsKey(remainAP))
            {
                dictionary[remainAP] = new List<T>();
            }
            dictionary[remainAP].Add(source);
        }
    }

    private void SetSpellDictionary()
    {
        SetDictionary(sourceDic, _spellSources);
    }

    private void SetMonsterDictionary()
    {
        SetDictionary(monsterDic, _monsters);
    }
    

    #endregion
    
    private Vector3 GetPosition(int sameAPCount, int remainSpellAP, int myIndex , float yPos)
    {
        Vector3 position = Vector3.zero;
        int guageMaxCount = _apGaugePos.Length - 1;
        position.y = yPos;
        
        var startPos = _apGaugePos[Math.Min(guageMaxCount, remainSpellAP - 1)].position;
        var endPos = _apGaugePos[Math.Min(guageMaxCount, remainSpellAP)].position;
        
        if (sameAPCount >= 2)
        {
            var delta = endPos - startPos;
            var pos = delta / (sameAPCount + 1);
            position.x = endPos.x + pos.x * (myIndex + 1);
        }
        else
        {
            position.x = (startPos.x + endPos.x) / 2;
        }

        return position;
    }
}
