using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReward : MonoBehaviour
{
    [Header("리워드 목록들")]
    [SerializeField] private UISpellSource spellSource;
    [SerializeField] private UIApplication uiApplication;
    [SerializeField] private UISupportModule uiSupportModule;
    
    public void SetReward(REWARD_TYPE rewardType, int id)
    {
        SetOffRewardAll();
        switch (rewardType)
        {
            case REWARD_TYPE.REWARD_TYPE_ITEM:
                break;
            case REWARD_TYPE.REWARD_TYPE_SPELL_SOURCE:
            {
                var source = GameTableManager.Instance._spellData.Find(_ => _.spell_id == id);
                spellSource.gameObject.SetActive(true);
                spellSource.SetImage(source.spell_img, source.spell_img);
            } break;
            case REWARD_TYPE.REWARD_TYPE_SUPPORT_MODULE:
            {
                var module = GameTableManager.Instance._supportModuleTable.Find(_=>_.support_module_id == id);
                uiSupportModule.gameObject.SetActive(true);
                uiSupportModule.SetImage(module.support_module_img);
            } break;
            case REWARD_TYPE.REWARD_TYPE_MONEY:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(rewardType), rewardType, null);
        }
    }
    
    private void SetOffRewardAll()
    {
        spellSource.gameObject.SetActive(false);
        uiSupportModule.gameObject.SetActive(false);
        uiApplication.gameObject.SetActive(false);
    }

}
