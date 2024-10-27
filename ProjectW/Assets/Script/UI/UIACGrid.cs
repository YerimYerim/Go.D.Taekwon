using System;
using System.Collections.Generic;
using UnityEngine;

public class UIACGrid : MonoBehaviour
{
    [SerializeField] UIAbnormal uiAbnormalPrefab;
    private ActorDataBase _actorDataBase;

    List<UIAbnormal> _uiTurnskill = new();

    
    /// <summary>
    /// 추후 재활용 시스템 구현시 고쳐야할 부분 
    /// </summary>
    /// <param name="dataBase"></param>
    public void Set(ActorDataBase dataBase)
    {
        _actorDataBase = dataBase;
        uiAbnormalPrefab.gameObject.SetActive(true);
        
        _uiTurnskill.ForEach(_=> Destroy(_.gameObject));
        _uiTurnskill.Clear();
        
        for (int i = 0; i < _actorDataBase.turnSkill.Count; i++)
        {
            var abnormal = _actorDataBase.turnSkill[i];
            var turnskill = abnormal as ISkillTurnSkill;
            EFFECT_TYPE? effectType = abnormal.GetTable().effect_type;
            var table = GameTableManager.Instance._abnormalTable.Find(_ => _.effect_type == effectType);
            
            UIAbnormal uiAbnormal = Instantiate(uiAbnormalPrefab, transform);
            uiAbnormal.SetImage(table?.abnormal_icon ?? String.Empty);
            uiAbnormal.SetText(turnskill?.GetRemainTime().ToString()?? string.Empty);
            uiAbnormal.SetBgColor(table?.abnormal_bg ?? String.Empty);
            uiAbnormal.SetData(table);
            _uiTurnskill.Add(uiAbnormal);
        }
        uiAbnormalPrefab.gameObject.SetActive(false);
    }
}
