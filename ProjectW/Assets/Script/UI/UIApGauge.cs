using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIApGauge : UIBase
{
    [SerializeField] Transform _startTransform;
    [SerializeField] Transform _endTransform;
    
    // 시작과 끝을 다섯칸으로 나눠준다
    [SerializeField] Transform[] _apGaugePos = new Transform[7];
    
    
    [SerializeField] UIAPGaugeIcon _apSourcePrefab;
    [SerializeField] GameObject _apEnemyPrefab;
    
    [SerializeField] Transform _apSourceParentsPos;
    
    Dictionary<int, List<UIAPGaugeIcon>> dictionary = new();
    //[SerializeField] List<
    // [0] [1] -> 쿨타임 1  - 1인게 여러개라면 개수만큼 해당 구역을 나눠서 배치
    // [1] [2] -> 쿨타임 2
    // [2] [3] -> 쿨타임 3
    // [3] [4] -> 쿨타임 4
    // [4] [5] -> 쿨타임 5
    
    
    // ap , spell
    private List<UIAPGaugeIcon> _spellSources = new();

    public void Init(List<GameSpellSource> spellSources)
    {
        SetSpellDictionary();
        foreach (var spell in spellSources)
        {
            var obj = Instantiate(_apSourcePrefab, _endTransform.position, Quaternion.identity,_apSourceParentsPos);
            var uiApGaugeIcon = obj.GetComponent<UIAPGaugeIcon>();
            uiApGaugeIcon.SetSpellSource(spell);
            _spellSources.Add(uiApGaugeIcon);
        }

        UpdateUI();
        _apSourcePrefab.gameObject.SetActive(false);
    }
    
    
    public void UpdateUI()
    {
        SetSpellDictionary();
        for(int i = 0; i< _spellSources.Count; i++)
        {
            int sameAPCount = 1;
            int myIndex = 0;
            if(dictionary.ContainsKey(_spellSources[i].GetRemainSpellAP()))
            {
                sameAPCount = dictionary[_spellSources[i].GetRemainSpellAP()].Count;
                myIndex = dictionary[_spellSources[i].GetRemainSpellAP()].FindIndex(_=>_ == _spellSources[i]);
            }
            
            var remainSpellAP = _spellSources[i].GetRemainSpellAP();
            Vector3 position  = Vector3.zero;
            if (sameAPCount >= 2)
            {
                var delta = _apGaugePos[Math.Min(5, remainSpellAP)].position - _apGaugePos[Math.Min(5, remainSpellAP - 1)].position;
                var pos = delta / (sameAPCount + 1);
                position = _apGaugePos[Math.Min(5, remainSpellAP)].position + pos * (myIndex+1);
            }
            else
            {
                position = (_apGaugePos[Math.Min(5,remainSpellAP - 1)].position + _apGaugePos[Math.Min(5,remainSpellAP)].position) / 2;
            }
            
            _spellSources[i].SetPositionSmooth(position);
        }

    }
    
    private void SetSpellDictionary()
    {
        dictionary.Clear();
        for (int i = 0; i < _spellSources.Count; i++)
        {
            _spellSources[i].GetRemainSpellAP();
            if (dictionary.ContainsKey(_spellSources[i].GetRemainSpellAP()) == false)
            {
                dictionary.Add(_spellSources[i].GetRemainSpellAP(), new List<UIAPGaugeIcon>(){_spellSources[i]});
            }
            else
            {
                dictionary[_spellSources[i].GetRemainSpellAP()].Add(_spellSources[i]);
            }
        }
    }
}
