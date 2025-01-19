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

    #region Spawn

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
        foreach (var monsterData in gameBattleMode.ActorHandler.GetEnemyData())
        {
            var obj = Instantiate(_apEnemyPrefab, _apGaugePos[^1].position, Quaternion.identity, _apEnemyParentsPos);
            var uiMonster = obj.GetComponent<UIAPGaugeIconMonster>();
            uiMonster.SetData(monsterData);
            _monsters.Add(uiMonster);
        }

        SetMonsterDictionary();
        _apEnemyPrefab.gameObject.SetActive(false);
    }
    

    #endregion
    
    #region Init

    public void Init()
    {
        ResetAll();
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        SpawnSourceIcon(gameBattleMode);
        SpawnMonsterIcon(gameBattleMode);
        UpdateUI(false);
        StartCoroutine(InitCoroutine());
    }
    /// <summary>
    /// 생성후 프레임이 지나지 않았을대 set하면 이상한 위치로 위치하여 0.1초 이후 set
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateUI();
    }
    

    #endregion

    #region Reset

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
    
    #endregion
    
    #region UpdateUI

    public void UpdateUI(bool isSmooth = true)
    {
        UpdateSourceUI(isSmooth);
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
            {
                if (sources[i].GetResetAp() == remainAP)
                {
                    var resetPosition = GetPosition(sameAPCount, _apGaugePos.Length - 1, myIndex, prefabTransform.position.y);
                    sources[i].SetPosition(resetPosition);
                }
                sources[i].AddPositionSmooth(position);
            }
            else
            {
                sources[i].SetPosition(position);
            }
        }
    }

    public void UpdateSourceUI(bool isSmooth)
    {
        SetSpellDictionary();
        UpdateUI(sourceDic, _spellSources, _apSourcePrefab.transform, isSmooth);
    }

    public void UpdateMonsterUI(bool isSmooth)
    {
        SetMonsterDictionary();
        UpdateUI(monsterDic, _monsters, _apEnemyPrefab.transform, isSmooth);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UpdateSourceUI(false);
        }
    }

    #endregion  

    #region SetDictionary

    private void SetDictionary<T>(IDictionary<int, List<T>> dictionary, List<T> sources) where T : UIAPGaugeIconBase
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
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        for (var i = 0; i < _spellSources.Count; i++)
        {
            _spellSources[i].SetSpellSource(gameBattleMode.BattleHandler._sources[i]);
        }
        SetDictionary(sourceDic, _spellSources);
    }

    private void SetMonsterDictionary()
    {
        var gameBattleMode = GameInstanceManager.Instance.GetGameMode<GameBattleMode>();
        for (var i = 0; i < _monsters.Count; i++)
        {
            var enemy = gameBattleMode.ActorHandler.GetEnemy(i);
            if (enemy == null || enemy.data.GetHp() <= 0 )
            {
                _monsters[i].gameObject.SetActive(false);
                _monsters[i].SetData(null);
            }
            else
            {
                _monsters[i].gameObject.SetActive(true);
                _monsters[i].SetData(enemy);
            }

        }
        SetDictionary(monsterDic, _monsters);
    }
    

    #endregion
    
    private Vector3 GetPosition(int sameAPCount, int remainSpellAP, int myIndex , float yPos)
    {
        Vector3 position = Vector3.zero;
        int guageMaxCount = _apGaugePos.Length - 1;
        position.y = yPos;
        
        var startPosIndex = Math.Min(guageMaxCount, Math.Max(0, remainSpellAP - 1));
        var endPosIndex = Math.Min(guageMaxCount, remainSpellAP);
        
        var startPos = _apGaugePos[startPosIndex].position;
        var endPos = _apGaugePos[endPosIndex].position;
        
        if (sameAPCount > 1)
        {
            var delta = endPos - startPos;
            var pos = delta / (sameAPCount + 1);
            position.x = startPos.x + pos.x * (myIndex + 1);
        }
        else
        {
            position.x = (startPos.x + endPos.x) / 2;
        }

        return position;
    }
    
}
