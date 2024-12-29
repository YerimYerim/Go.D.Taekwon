using System;
using Script.UI;
using TMPro;
using UnityEngine;

public class UIPopUpRewardSelectObject : MonoBehaviour
{
    [SerializeField] private DTButton button;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI desc;
    
    [Header("리워드 목록들")]
    [SerializeField] private UISpellSource spellSource;
    [SerializeField] private UIApplication uiApplication;
    [SerializeField] private UISupportModule uiSupportModule;

    public void SetData(REWARD_TYPE rewardType, int id)
    {
        SetReward(rewardType, id);
    }

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
                this.title.text = GameUtil.GetString(source.spell_name);
                this.desc.text = GameUtil.GetString(source.spell_desc);
                spellSource.SetImage(source.spell_img, source.spell_img);
                spellSource.gameObject.SetActive(true);
                
            } break;
            case REWARD_TYPE.REWARD_TYPE_SUPPORT_MODULE:
            {
                var module = GameTableManager.Instance._supportModuleTable.Find(_=>_.support_module_id == id);
                this.title.text = GameUtil.GetString(module.support_module_name);
                this.desc.text = GameUtil.GetString(module.support_module_desc);
                uiSupportModule.SetImage(module.support_module_img);
                uiSupportModule.gameObject.SetActive(true);
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
    
    public void SetButtonEvent(System.Action action)
    {
        button.onClick.AddListener(() => action());
    }

}
